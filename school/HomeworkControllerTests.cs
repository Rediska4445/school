using NUnit.Framework;
using school.Controllers;
using school.Models;
using Microsoft.Data.SqlClient;
using System;
using NUnit.Framework.Legacy;
using System.ComponentModel.DataAnnotations;

namespace school.Tests.Integration
{
    //| Тест                                              | Сценарий                           | Уникальность | FK |
    //| ------------------------------------------------- | ---------------------------------- | ------------ | -- |
    //| InsertOrUpdateHomework_NewHomework                | ✅ Создание новой                   |              | ✅  |
    //| InsertOrUpdateHomework_ExistingByDateSubjectClass | ✅ Обновление по дата+класс+предмет | ✅            | ✅  |
    //| GetHomeworkBySubjectAndDate_Existing              | ✅ Поиск по предмет+дата+класс      | ✅            |    |
    //| DeleteHomework_ExistingHomework                   | ✅ Удаление                         |              | ✅  |
    //| InsertOrUpdateHomework_InvalidData                | ✅ Валидация                        |              |    |
    //| GetHomeworkById_ExistingId                        | ✅ Поиск по ID                      |              |    |
    //| GetHomeworkForPeriod_WithHomework                 | ✅ Получение за период (1 запись)   |              |    |
    //| GetHomeworkForPeriod_EmptyPeriod                  | ✅ Пустой период → пустой список    |              |    |
    //| GetHomeworkForPeriod_MultipleHomework             | ✅ Несколько записей, сортировка    | ✅            |    |

    [TestFixture]
    public class HomeworkControllerTests
    {
        private HomeworkController _controller;
        private string _connectionString;

        [SetUp]
        public void Setup()
        {
            _connectionString = Form1.CONNECTION_STRING;
            _controller = new HomeworkController(_connectionString);
            CleanupTestData();
        }

        [TearDown]
        public void TearDown()
        {
            CleanupTestData();
        }

        private void CleanupTestData()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("DELETE FROM Homework", conn)) cmd.ExecuteNonQuery();
                using (var cmd = new SqlCommand("DELETE FROM Users", conn)) cmd.ExecuteNonQuery();
                using (var cmd = new SqlCommand("DELETE FROM Subjects", conn)) cmd.ExecuteNonQuery();
                using (var cmd = new SqlCommand("DELETE FROM Classes", conn)) cmd.ExecuteNonQuery();
            }
        }

        // 🔥 ТЕСТ ДЛЯ ОТЛАДКИ SQL
        [Test]
        public void DebugSqlQuery()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Classes WHERE ClassID = 1", conn))
                {
                    int classCount = (int)cmd.ExecuteScalar();
                    Console.WriteLine($"🔍 Классов ID=1: {classCount}");
                }

                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Schedule WHERE ClassID = 1", conn))
                {
                    int scheduleCount = (int)cmd.ExecuteScalar();
                    Console.WriteLine($"🔍 Расписания для класса 1: {scheduleCount}");
                }

                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Homework WHERE ClassID = 1 AND AssignmentDate = '2025-12-08'", conn))
                {
                    int homeworkCount = (int)cmd.ExecuteScalar();
                    Console.WriteLine($"🔍 ДЗ на 08.12 для класса 1: {homeworkCount}");
                }
            }
        }

        [Test]
        public void InsertOrUpdateHomework_NewHomework_ReturnsNewId()
        {
            // Arrange: Создаём зависимости
            int classId = CreateClass();
            int subjectId = CreateSubject();
            int teacherId = CreateTeacher();

            var homework = new Homework
            {
                AssignmentDate = new DateTime(2025, 1, 15),
                ClassID = classId,
                SubjectID = subjectId,
                Description = "§1-3, повторить формулы",
                TeacherID = teacherId
            };

            // Act
            int resultId = _controller.InsertOrUpdateHomework(homework);

            // Assert
            Assert.That(resultId, Is.GreaterThan(0));
            Assert.That(homework.HomeworkID, Is.EqualTo(resultId));

            var saved = _controller.GetHomeworkById(resultId);
            Assert.That(saved, Is.Not.Null);
            Assert.That(saved.Description, Is.EqualTo("§1-3, повторить формулы"));
        }

        [Test]
        public void InsertOrUpdateHomework_ExistingByDateSubjectClass_UpdatesDescription()
        {
            // Arrange: Создаём зависимости + двух учителей
            int classId = CreateClass();
            int subjectId = CreateSubject();
            int teacherId1 = CreateTeacher();  // Первый учитель
            int teacherId2 = CreateTeacher();  // Второй учитель (гарантированно существует)

            var date = new DateTime(2025, 1, 15);

            // 1. Создаём первую домашку
            var homework1 = new Homework
            {
                AssignmentDate = date,
                ClassID = classId,
                SubjectID = subjectId,
                Description = "Первая версия",
                TeacherID = teacherId1
            };
            _controller.InsertOrUpdateHomework(homework1);

            // Act: Обновляем ту же комбинацию дата+класс+предмет (другой учитель)
            var homework2 = new Homework
            {
                AssignmentDate = date,
                ClassID = classId,
                SubjectID = subjectId,
                Description = "Обновлённая версия §1-3 ✅",
                TeacherID = teacherId2  // ✅ Существующий учитель!
            };
            int updatedId = _controller.InsertOrUpdateHomework(homework2);

            // Assert
            var updated = _controller.GetHomeworkById(updatedId);
            Assert.That(updated.Description, Is.EqualTo("Обновлённая версия §1-3 ✅"));
            Assert.That(updated.TeacherID, Is.EqualTo(teacherId2));
            Assert.That(updatedId, Is.GreaterThan(0));
        }

        [Test]
        public void GetHomeworkBySubjectAndDate_Existing_ReturnsHomework()
        {
            // Arrange
            int classId = CreateClass();
            int subjectId = CreateSubject();
            int teacherId = CreateTeacher();

            var homework = new Homework
            {
                AssignmentDate = new DateTime(2025, 1, 15),
                ClassID = classId,
                SubjectID = subjectId,
                Description = "Найти по предмету+дате",
                TeacherID = teacherId
            };
            int hwId = _controller.InsertOrUpdateHomework(homework);

            // Act
            var found = _controller.GetHomeworkBySubjectAndDate(subjectId, new DateTime(2025, 1, 15), classId);

            // Assert
            Assert.That(found, Is.Not.Null);
            Assert.That(found.HomeworkID, Is.EqualTo(hwId));
            Assert.That(found.SubjectID, Is.EqualTo(subjectId));
        }

        [Test]
        public void GetHomeworkBySubjectAndDate_NonExisting_ReturnsNull()
        {
            var result = _controller.GetHomeworkBySubjectAndDate(999, DateTime.Now, 999);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void DeleteHomework_ExistingHomework_ReturnsTrue()
        {
            // Arrange
            int classId = CreateClass();
            int subjectId = CreateSubject();
            int teacherId = CreateTeacher();

            var homework = new Homework
            {
                AssignmentDate = DateTime.Now,
                ClassID = classId,
                SubjectID = subjectId,
                Description = "Удалить",
                TeacherID = teacherId
            };
            int hwId = _controller.InsertOrUpdateHomework(homework);

            // Act
            bool deleted = _controller.DeleteHomework(homework);

            // Assert
            Assert.That(deleted, Is.True);
            var deletedHw = _controller.GetHomeworkById(hwId);
            Assert.That(deletedHw, Is.Null);
        }

        [Test]
        public void InsertOrUpdateHomework_NullHomework_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _controller.InsertOrUpdateHomework(null));
        }

        [Test]
        public void InsertOrUpdateHomework_EmptyDescription_ThrowsValidationException()
        {
            var homework = new Homework
            {
                AssignmentDate = DateTime.Now,
                ClassID = 1,
                SubjectID = 1,
                Description = "",  // ❌ Required
                TeacherID = 1
            };

            var ex = Assert.Throws<ValidationException>(() => _controller.InsertOrUpdateHomework(homework));
            StringAssert.Contains("Description", ex.Message);
        }

        [Test]
        public void InsertOrUpdateHomework_TooLongDescription_ThrowsValidationException()
        {
            var homework = new Homework
            {
                AssignmentDate = DateTime.Now,
                ClassID = 1,
                SubjectID = 1,
                Description = new string('А', 501),  // ❌ StringLength(500)
                TeacherID = 1
            };

            var ex = Assert.Throws<ValidationException>(() => _controller.InsertOrUpdateHomework(homework));
            StringAssert.Contains("Description", ex.Message);
        }

        [Test]
        public void GetHomeworkById_ExistingId_ReturnsHomework()
        {
            // Arrange
            int classId = CreateClass();
            int subjectId = CreateSubject();
            int teacherId = CreateTeacher();

            var homework = new Homework
            {
                AssignmentDate = DateTime.Now,
                ClassID = classId,
                SubjectID = subjectId,
                Description = "По ID",
                TeacherID = teacherId
            };
            int hwId = _controller.InsertOrUpdateHomework(homework);

            // Act
            var found = _controller.GetHomeworkById(hwId);

            // Assert
            Assert.That(found, Is.Not.Null);
            Assert.That(found.Description, Is.EqualTo("По ID"));
        }

        [Test]
        public void GetHomeworkById_NonExistingId_ReturnsNull()
        {
            var result = _controller.GetHomeworkById(999);
            Assert.That(result, Is.Null);
        }

        // ✅ НОВЫЕ ТЕСТЫ ДЛЯ GetHomeworkForPeriod
        [Test]
        public void GetHomeworkForPeriod_WithHomework_ReturnsCorrectList()
        {
            // Arrange
            int classId = CreateClass();
            int subjectId = CreateSubject();
            int teacherId = CreateTeacher();

            var homework = new Homework
            {
                AssignmentDate = new DateTime(2025, 12, 7),
                ClassID = classId,
                SubjectID = subjectId,
                Description = "Решить задачи 1-10",
                TeacherID = teacherId
            };
            int hwId = _controller.InsertOrUpdateHomework(homework);

            // Act
            var homeworkList = _controller.GetHomeworkForPeriod(new DateTime(2025, 12, 1), new DateTime(2025, 12, 31));

            // Assert
            Assert.That(homeworkList.Count, Is.EqualTo(1));
            Assert.That(homeworkList[0].HomeworkID, Is.EqualTo(hwId));
            Assert.That(homeworkList[0].Description, Is.EqualTo("Решить задачи 1-10"));
            Assert.That(homeworkList[0].ClassID, Is.EqualTo(classId));
        }

        [Test]
        public void GetHomeworkForPeriod_EmptyPeriod_ReturnsEmptyList()
        {
            // Act
            var homeworkList = _controller.GetHomeworkForPeriod(new DateTime(2025, 1, 1), new DateTime(2025, 1, 31));

            // Assert
            Assert.That(homeworkList.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetHomeworkForPeriod_MultipleHomework_ReturnsSortedList()
        {
            // Arrange
            int classId = CreateClass();
            int subjectId = CreateSubject();
            int teacherId = CreateTeacher();

            // Первая домашка (5 декабря)
            _controller.InsertOrUpdateHomework(new Homework
            {
                AssignmentDate = new DateTime(2025, 12, 5),
                ClassID = classId,
                SubjectID = subjectId,
                Description = "Задание 1",
                TeacherID = teacherId
            });

            // Вторая домашка (7 декабря)
            _controller.InsertOrUpdateHomework(new Homework
            {
                AssignmentDate = new DateTime(2025, 12, 7),
                ClassID = classId,
                SubjectID = subjectId,
                Description = "Задание 2",
                TeacherID = teacherId
            });

            // Act
            var homeworkList = _controller.GetHomeworkForPeriod(new DateTime(2025, 12, 1), new DateTime(2025, 12, 31));

            // Assert
            Assert.That(homeworkList.Count, Is.EqualTo(2));
            Assert.That(homeworkList[0].AssignmentDate.Day, Is.EqualTo(5));  // Первое по дате
            Assert.That(homeworkList[1].AssignmentDate.Day, Is.EqualTo(7));  // Второе по дате
        }

        // ✅ Вспомогательные методы для создания тестовых данных
        private int CreateClass()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("INSERT INTO Classes (ClassName) OUTPUT INSERTED.ClassID VALUES ('10А')", conn))
                {
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        private int CreateSubject()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("INSERT INTO Subjects (SubjectName) OUTPUT INSERTED.SubjectID VALUES ('Математика')", conn))
                {
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        private int CreateTeacher()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("INSERT INTO Users (FullName, Role) OUTPUT INSERTED.UserID VALUES ('Тест Учитель', 'Учитель')", conn))
                {
                    return (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
