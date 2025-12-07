using NUnit.Framework;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework.Legacy;
using school.Models;
using school;
using System;

namespace school.Tests.Integration
{
    [TestFixture]
    public class SubjectControllerTests
    {
        private SubjectController _controller;
        private string _connectionString;

        [SetUp]
        public void Setup()
        {
            _connectionString = Form1.CONNECTION_STRING;
            _controller = new SubjectController(_connectionString);
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
                using (var cmd = new SqlCommand("DELETE FROM Grades WHERE SubjectID IN (SELECT SubjectID FROM Subjects)", conn)) cmd.ExecuteNonQuery();
                using (var cmd = new SqlCommand("DELETE FROM Homework WHERE SubjectID IN (SELECT SubjectID FROM Subjects)", conn)) cmd.ExecuteNonQuery();
                using (var cmd = new SqlCommand("DELETE FROM Subjects", conn)) cmd.ExecuteNonQuery();
            }
        }

        [Test]
        public void InsertOrUpdateSubject_NewSubject_ReturnsNewId()
        {
            // Arrange
            var newSubject = new Subject { SubjectName = "Математика" };  // ✅ Без N

            // Act
            int resultId = _controller.InsertOrUpdateSubject(newSubject);

            // Assert
            Assert.That(resultId, Is.GreaterThan(0));
            Assert.That(newSubject.SubjectID, Is.EqualTo(resultId));

            var savedSubject = _controller.GetSubjectById(resultId);
            Assert.That(savedSubject, Is.Not.Null);
            Assert.That(savedSubject.SubjectName, Is.EqualTo("Математика"));  // ✅ Без N
        }

        [Test]
        public void InsertOrUpdateSubject_UpdateExistingSubject_UpdatesName()
        {
            // Arrange
            var newSubject = new Subject { SubjectName = "Физика" };
            int id = _controller.InsertOrUpdateSubject(newSubject);

            // Act
            newSubject.SubjectName = "Химия";
            newSubject.SubjectID = id;
            _controller.InsertOrUpdateSubject(newSubject);

            // Assert
            var updatedSubject = _controller.GetSubjectById(id);
            Assert.That(updatedSubject.SubjectName, Is.EqualTo("Химия"));
        }

        [Test]
        public void GetSubjectById_ExistingId_ReturnsSubject()
        {
            // Arrange
            var testSubject = new Subject { SubjectName = "Информатика" };
            int id = _controller.InsertOrUpdateSubject(testSubject);

            // Act
            var foundSubject = _controller.GetSubjectById(id);

            // Assert
            Assert.That(foundSubject, Is.Not.Null);
            Assert.That(foundSubject.SubjectID, Is.EqualTo(id));
            Assert.That(foundSubject.SubjectName, Is.EqualTo("Информатика"));
        }

        [Test]
        public void GetSubjectById_NonExistingId_ReturnsNull()
        {
            var result = _controller.GetSubjectById(999);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void DeleteSubject_ExistingSubjectWithoutDependencies_ReturnsTrue()
        {
            // Arrange
            var testSubject = new Subject { SubjectName = "История" };
            int id = _controller.InsertOrUpdateSubject(testSubject);

            // Act
            bool deleted = _controller.DeleteSubject(testSubject);

            // Assert
            Assert.That(deleted, Is.True);
            var deletedSubject = _controller.GetSubjectById(id);
            Assert.That(deletedSubject, Is.Null);
        }

        [Test]
        public void DeleteSubject_NonExistingSubject_ReturnsFalse()
        {
            var nonExistingSubject = new Subject { SubjectID = 999 };
            bool deleted = _controller.DeleteSubject(nonExistingSubject);
            Assert.That(deleted, Is.False);
        }

        [Test]
        public void InsertOrUpdateSubject_InvalidSubject_ThrowsArgumentException()
        {
            var invalidSubject = new Subject { SubjectName = "" };
            Assert.Throws<ArgumentException>(() => _controller.InsertOrUpdateSubject(invalidSubject));
        }

        [Test]
        public void InsertOrUpdateSubject_NullSubject_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _controller.InsertOrUpdateSubject(null));
        }

        [Test]
        public void DeleteSubject_WithHomeworkDependency_ThrowsInvalidOperationException()
        {
            // Arrange: Создаём предмет
            var testSubject = new Subject { SubjectName = "Биология" };
            int subjectId = _controller.InsertOrUpdateSubject(testSubject);

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // 1. Создаём класс
                int classId;
                using (var cmd = new SqlCommand("INSERT INTO Classes (ClassName) OUTPUT INSERTED.ClassID VALUES (@ClassName)", conn))
                {
                    cmd.Parameters.AddWithValue("@ClassName", "10А");
                    classId = (int)cmd.ExecuteScalar();
                }

                // 2. Создаём учителя
                int teacherId;
                using (var cmd = new SqlCommand("INSERT INTO Users (FullName, Role) OUTPUT INSERTED.UserID VALUES (@FullName, @Role)", conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", "Тест Учитель");
                    cmd.Parameters.AddWithValue("@Role", "Учитель");
                    teacherId = (int)cmd.ExecuteScalar();
                }

                // 3. ДЗ с этим предметом
                using (var cmd = new SqlCommand(@"
            INSERT INTO Homework (AssignmentDate, ClassID, SubjectID, Description, TeacherID) 
            VALUES (@Date, @ClassID, @SubjectID, @Description, @TeacherID)", conn))
                {
                    cmd.Parameters.AddWithValue("@Date", "2025-01-01");
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                    cmd.Parameters.AddWithValue("@Description", "Тестовое задание");
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                    cmd.ExecuteNonQuery();
                }

                // 4. Создаём ученика
                int studentId;
                using (var cmd = new SqlCommand("INSERT INTO Users (FullName, Role, ClassID) OUTPUT INSERTED.UserID VALUES (@FullName, @Role, @ClassID)", conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", "Тест Ученик");
                    cmd.Parameters.AddWithValue("@Role", "Ученик");
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    studentId = (int)cmd.ExecuteScalar();
                }

                // 5. Оценка с этим предметом
                using (var cmd = new SqlCommand(@"
            INSERT INTO Grades (GradeDate, StudentID, SubjectID, GradeValue, TeacherID) 
            VALUES (@Date, @StudentID, @SubjectID, @GradeValue, @TeacherID)", conn))
                {
                    cmd.Parameters.AddWithValue("@Date", "2025-01-01");
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                    cmd.Parameters.AddWithValue("@GradeValue", (byte)5);
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                    cmd.ExecuteNonQuery();
                }
            }

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _controller.DeleteSubject(testSubject));

            // ✅ ПРОВЕРЯЕМ ТОЧНЫЙ текст из контроллера
            Assert.That(ex.Message, Does.Contain("Нельзя удалить предмет"));
            Assert.That(ex.Message, Does.Contain("ДЗ (1)"));
            Assert.That(ex.Message, Does.Contain("оценки (1)"));

            // ИЛИ полная проверка
            StringAssert.Contains("Есть ДЗ", ex.Message);
            StringAssert.Contains("и оценки", ex.Message);
        }

        [Test]
        public void InsertOrUpdateSubject_UsedInHomeworkAndGrades_ThrowsInvalidOperationException()
        {
            // Arrange: Создаём предмет с зависимостями
            var testSubject = new Subject { SubjectName = "География" };
            int subjectId = _controller.InsertOrUpdateSubject(testSubject);

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // 1. Создаём класс
                int classId;
                using (var cmd = new SqlCommand("INSERT INTO Classes (ClassName) OUTPUT INSERTED.ClassID VALUES (@ClassName)", conn))
                {
                    cmd.Parameters.AddWithValue("@ClassName", "10Б");
                    classId = (int)cmd.ExecuteScalar();
                }

                // 2. Создаём учителя
                int teacherId;
                using (var cmd = new SqlCommand("INSERT INTO Users (FullName, Role) OUTPUT INSERTED.UserID VALUES (@FullName, @Role)", conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", "Учитель2");
                    cmd.Parameters.AddWithValue("@Role", "Учитель");
                    teacherId = (int)cmd.ExecuteScalar();
                }

                // 3. Создаём ученика
                int studentId;
                using (var cmd = new SqlCommand("INSERT INTO Users (FullName, Role, ClassID) OUTPUT INSERTED.UserID VALUES (@FullName, @Role, @ClassID)", conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", "Ученик2");
                    cmd.Parameters.AddWithValue("@Role", "Ученик");
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    studentId = (int)cmd.ExecuteScalar();
                }

                // 4. ДЗ с этим предметом
                using (var cmd = new SqlCommand(@"
            INSERT INTO Homework (AssignmentDate, ClassID, SubjectID, Description, TeacherID) 
            VALUES (@Date, @ClassID, @SubjectID, @Description, @TeacherID)", conn))
                {
                    cmd.Parameters.AddWithValue("@Date", "2025-01-02");
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                    cmd.Parameters.AddWithValue("@Description", "ДЗ2");
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                    cmd.ExecuteNonQuery();
                }

                // 5. Оценка с этим предметом
                using (var cmd = new SqlCommand(@"
            INSERT INTO Grades (GradeDate, StudentID, SubjectID, GradeValue, TeacherID) 
            VALUES (@Date, @StudentID, @SubjectID, @GradeValue, @TeacherID)", conn))
                {
                    cmd.Parameters.AddWithValue("@Date", "2025-01-02");
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                    cmd.Parameters.AddWithValue("@GradeValue", (byte)4);
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                    cmd.ExecuteNonQuery();
                }
            }

            // Act: Пытаемся обновить название предмета
            Console.WriteLine($"SubjectID перед обновлением: {subjectId}");

            // Act
            testSubject.SubjectName = "Изменённая География";
            testSubject.SubjectID = subjectId;

            Console.WriteLine($"Попытка обновить SubjectID={testSubject.SubjectID}, Name='{testSubject.SubjectName}'");

            // Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _controller.InsertOrUpdateSubject(testSubject));

            Console.WriteLine($"Исключение: {ex.Message}");

            StringAssert.Contains("ДЗ", ex.Message);
            StringAssert.Contains("оценках", ex.Message);
        }
    }

    public static class SqlExtensions
    {
        public static object ExecuteScalarWithParams(this SqlCommand cmd, SqlConnection conn, params object[] values)
        {
            for (int i = 0; i < values.Length; i++)
                cmd.Parameters.AddWithValue($"@p{i}", values[i]);
            return cmd.ExecuteScalar();
        }
    }
}
