using NUnit.Framework;
using school.Controllers;
using school.Models;
using System;
using System.Linq;

namespace school.Tests.Integration
{
    [TestFixture]
    public class TeacherControllerTests
    {
        private TeacherController _controller;
        private string TEST_CONNECTION_STRING = Form1.CONNECTION_STRING;
        private const int TEST_TEACHER_ID = 1; // Иванов - должен существовать в БД

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _controller = TeacherController._controller;
        }

        [Test]
        public void GetTeacherHomework_ReturnsCorrectData_ForValidPeriod()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(-14).Date;
            var endDate = DateTime.Now.Date;
            var teacher = new User { UserID = TEST_TEACHER_ID };

            // Act
            var homeworkList = _controller.GetTeacherHomework(startDate, endDate, teacher);

            // Assert
            Assert.That(homeworkList, Is.Not.Null);

            // Проверяем, что все записи принадлежат учителю
            Assert.That(homeworkList.All(h => h.TeacherID == TEST_TEACHER_ID), Is.True);

            // Проверяем даты
            Assert.That(homeworkList.All(h => h.AssignmentDate >= startDate && h.AssignmentDate <= endDate), Is.True);

            // Проверяем заполненность обязательных полей
            Assert.That(homeworkList.All(h => !string.IsNullOrEmpty(h.Description)), Is.True);
            Assert.That(homeworkList.All(h => !string.IsNullOrEmpty(h.ClassNameDisplay)), Is.True);
            Assert.That(homeworkList.All(h => !string.IsNullOrEmpty(h.SubjectNameDisplay)), Is.True);
        }

        [Test]
        public void GetTeacherGrades_ReturnsCorrectData_ForValidPeriod()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(-14).Date;
            var endDate = DateTime.Now.Date;
            var teacher = new User { UserID = TEST_TEACHER_ID };

            // Act
            var gradesList = _controller.GetTeacherGrades(startDate, endDate, teacher);

            // Assert
            Assert.That(gradesList, Is.Not.Null);

            // Проверяем принадлежность учителю
            Assert.That(gradesList.All(g => g.TeacherID == TEST_TEACHER_ID), Is.True);

            // Проверяем даты
            Assert.That(gradesList.All(g => g.GradeDate >= startDate && g.GradeDate <= endDate), Is.True);

            // Проверяем диапазон оценок 1-5
            Assert.That(gradesList.All(g => g.GradeValue >= 1 && g.GradeValue <= 5), Is.True);

            // Проверяем заполненность
            Assert.That(gradesList.All(g => !string.IsNullOrEmpty(g.SubjectNameDisplay)), Is.True);
            Assert.That(gradesList.All(g => !string.IsNullOrEmpty(g.StudentNameDisplay)), Is.True);
        }

        [Test]
        public void GetTeacherHomework_ReturnsEmpty_ForFuturePeriod()
        {
            // Arrange
            var startDate = new DateTime(2025, 12, 10).Date; // Будущее относительно тестовых данных
            var endDate = new DateTime(2025, 12, 15).Date;
            var teacher = new User { UserID = TEST_TEACHER_ID };

            // Act
            var homeworkList = _controller.GetTeacherHomework(startDate, endDate, teacher);

            // Assert - ЛЮБОЙ из вариантов работает
            Assert.That(homeworkList, Is.Empty);                    // ✅ Вариант 1
                                                                    // Assert.That(homeworkList.Count, Is.EqualTo(0));      // ✅ Вариант 2  
                                                                    // Assert.That(!homeworkList.Any(), Is.True);           // ✅ Вариант 3
        }


        [Test]
        public void GetTeacherGrades_ReturnsEmptyList_ForNoDataPeriod()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(1).Date;
            var endDate = DateTime.Now.AddDays(7).Date;
            var teacher = new User { UserID = TEST_TEACHER_ID };

            // Act
            var gradesList = _controller.GetTeacherGrades(startDate, endDate, teacher);

            // Assert
            Assert.That(gradesList, Is.Not.Null);
            Assert.That(gradesList, Is.Empty);
        }

        [Test]
        public void GetTeacherHomework_SortedByDateDescending()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(-30).Date;
            var endDate = DateTime.Now.Date;
            var teacher = new User { UserID = TEST_TEACHER_ID };

            // Act
            var homeworkList = _controller.GetTeacherHomework(startDate, endDate, teacher);

            // Assert - сортировка по убыванию даты
            if (homeworkList.Count >= 2)
            {
                Assert.That(homeworkList[0].AssignmentDate, Is.GreaterThanOrEqualTo(homeworkList[1].AssignmentDate));
            }
        }

        [Test]
        public void GetTeacherGrades_SortedByDateDescending()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(-30).Date;
            var endDate = DateTime.Now.Date;
            var teacher = new User { UserID = TEST_TEACHER_ID };

            // Act
            var gradesList = _controller.GetTeacherGrades(startDate, endDate, teacher);

            // Assert - сортировка по убыванию даты
            if (gradesList.Count >= 2)
            {
                Assert.That(gradesList[0].GradeDate, Is.GreaterThanOrEqualTo(gradesList[1].GradeDate));
            }
        }

        [Test]
        public void GetTeacherHomework_HandlesInvalidTeacherId()
        {
            // Arrange - несуществующий учитель
            var startDate = DateTime.Now.AddDays(-7).Date;
            var endDate = DateTime.Now.Date;
            var invalidTeacher = new User { UserID = 999 };

            // Act
            var homeworkList = _controller.GetTeacherHomework(startDate, endDate, invalidTeacher);

            // Assert
            Assert.That(homeworkList, Is.Not.Null);
            Assert.That(homeworkList, Is.Empty);
        }

        [Test]
        public void UpsertHomework_InsertNew_ReturnsNewId()
        {
            // Arrange
            var testDate = DateTime.Now.Date;
            var homework = new Homework
            {
                AssignmentDate = testDate,
                ClassID = 1, // 7А
                SubjectID = 1, // Математика
                Description = "Тестовое ДЗ - INSERT",
                TeacherID = TEST_TEACHER_ID
            };

            // Act
            var id = _controller.UpsertHomework(homework);

            // Assert
            Assert.That(id, Is.GreaterThan(0));

            // Проверяем, что запись появилась
            var resultList = _controller.GetTeacherHomework(testDate, testDate, new User { UserID = TEST_TEACHER_ID });
            var createdHomework = resultList.FirstOrDefault(h => h.HomeworkID == id);

            Assert.That(createdHomework, Is.Not.Null);
            Assert.That(createdHomework.Description, Is.EqualTo("Тестовое ДЗ - INSERT"));
            Assert.That(createdHomework.ClassID, Is.EqualTo(1));
            Assert.That(createdHomework.SubjectID, Is.EqualTo(1));
        }

        [Test]
        public void UpsertHomework_UpdateExisting_UpdatesDescription()
        {
            // Arrange: создаем тестовое ДЗ
            var testDate = DateTime.Now.Date;
            var originalHomework = new Homework
            {
                AssignmentDate = testDate,
                ClassID = 1,
                SubjectID = 1,
                Description = "Исходное описание",
                TeacherID = TEST_TEACHER_ID
            };

            int homeworkId = _controller.UpsertHomework(originalHomework);

            // Обновляем описание
            var updatedHomework = new Homework
            {
                AssignmentDate = testDate,
                ClassID = 1,
                SubjectID = 1,
                Description = "Обновленное описание",
                TeacherID = TEST_TEACHER_ID
            };

            // Act
            int updatedId = _controller.UpsertHomework(updatedHomework);

            // Assert
            Assert.That(updatedId, Is.EqualTo(homeworkId)); // Тот же ID

            // Проверяем обновление в БД
            var resultList = _controller.GetTeacherHomework(testDate, testDate, new User { UserID = TEST_TEACHER_ID });
            var updatedRecord = resultList.FirstOrDefault(h => h.HomeworkID == homeworkId);

            Assert.That(updatedRecord.Description, Is.EqualTo("Обновленное описание"));
        }

        [Test]
        public void UpsertHomework_NoPermission_ThrowsException()
        {
            // Arrange: предмет, который учитель не ведет
            var homework = new Homework
            {
                AssignmentDate = DateTime.Now.Date,
                ClassID = 1,
                SubjectID = 999, // Неведомый предмет
                Description = "Запрещенное ДЗ",
                TeacherID = TEST_TEACHER_ID
            };

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _controller.UpsertHomework(homework));
            Assert.That(ex.Message, Contains.Substring("не ведет этот предмет"));
        }

        [Test]
        public void UpsertHomework_SameKeyMultipleCalls_ReturnsSameId()
        {
            // Arrange
            var testDate = DateTime.Now.Date;
            var homework1 = new Homework
            {
                AssignmentDate = testDate,
                ClassID = 1,
                SubjectID = 1,
                Description = "Первое описание",
                TeacherID = TEST_TEACHER_ID
            };

            var homework2 = new Homework
            {
                AssignmentDate = testDate,
                ClassID = 1,
                SubjectID = 1,
                Description = "Второе описание",
                TeacherID = TEST_TEACHER_ID
            };

            // Act: первый вызов (INSERT)
            int id1 = _controller.UpsertHomework(homework1);

            // Второй вызов (UPDATE)
            int id2 = _controller.UpsertHomework(homework2);

            // Assert
            Assert.That(id1, Is.GreaterThan(0));
            Assert.That(id2, Is.EqualTo(id1)); // Тот же ID!
        }

    }
}
