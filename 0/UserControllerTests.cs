using NUnit.Framework;
using school.Controllers;
using school.Models;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System;
using NUnit.Framework.Legacy;

namespace school.Tests.Integration
{
//| Тест                                       | Сценарий                      | FK проверка |
//| ------------------------------------------ | ----------------------------  | ----------- |
//| InsertOrUpdateUser_NewTeacher              | ✅ Создание учителя           |             |
//| InsertOrUpdateUser_NewStudentWithClass     | ✅ Создание ученика с классом |             |
//| InsertOrUpdateUser_UpdateExistingUser      | ✅ Обновление                 |             |
//| GetUserById_ExistingId                     | ✅ Получение по ID            |             |
//| DeleteUser_ExistingUserWithoutDependencies | ✅ Удаление без FK            |             |
//| DeleteUser_WithHomeworkDependency          | ✅ Блокировка FK ДЗ           | ✅          |
//| InsertOrUpdateUser_InvalidRole             | ✅ Валидация Role             |             |
//| GetUserByLoginPassword_ExistingUser        | ✅ Авторизация                |             |

    [TestFixture]
    public class UserControllerTests
    {
        private UserController _controller;
        private string _connectionString;

        [SetUp]
        public void Setup()
        {
            _connectionString = Form1.CONNECTION_STRING;
            _controller = new UserController(_connectionString);
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
                using (var cmd = new SqlCommand("DELETE FROM Grades", conn)) cmd.ExecuteNonQuery();
                using (var cmd = new SqlCommand("DELETE FROM Homework", conn)) cmd.ExecuteNonQuery();
                using (var cmd = new SqlCommand("DELETE FROM Users", conn)) cmd.ExecuteNonQuery();
                using (var cmd = new SqlCommand("DELETE FROM Subjects", conn)) cmd.ExecuteNonQuery();
                using (var cmd = new SqlCommand("DELETE FROM Classes", conn)) cmd.ExecuteNonQuery();
            }
        }

        [Test]
        public void InsertOrUpdateUser_NewTeacher_ReturnsNewId()
        {
            // Arrange
            var teacher = new User { FullName = "Иванов Иван Иванович", Role = "Учитель" };

            // Act
            int resultId = _controller.InsertOrUpdateUser(teacher);

            // Assert
            Assert.That(resultId, Is.GreaterThan(0));
            Assert.That(teacher.UserID, Is.EqualTo(resultId));

            var savedUser = _controller.GetUserById(resultId);
            Assert.That(savedUser, Is.Not.Null);
            Assert.That(savedUser.FullName, Is.EqualTo("Иванов Иван Иванович"));
            Assert.That(savedUser.Role, Is.EqualTo("Учитель"));
            Assert.That(savedUser.ClassID, Is.Null);
        }

        [Test]
        public void InsertOrUpdateUser_NewStudentWithClass_ReturnsNewId()
        {
            // Arrange: Создаём класс
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("INSERT INTO Classes (ClassName) OUTPUT INSERTED.ClassID VALUES ('10А')", conn))
                {
                    int classId = (int)cmd.ExecuteScalar();

                    // Act
                    var student = new User { FullName = "Петров Петр", Role = "Ученик", ClassID = classId };
                    int userId = _controller.InsertOrUpdateUser(student);

                    // Assert
                    Assert.That(userId, Is.GreaterThan(0));
                    var savedUser = _controller.GetUserById(userId);
                    Assert.That(savedUser.ClassID, Is.EqualTo(classId));
                }
            }
        }

        [Test]
        public void InsertOrUpdateUser_UpdateExistingUser_UpdatesData()
        {
            // Arrange
            var user = new User { FullName = "Сидоров Сидор", Role = "Ученик" };
            int id = _controller.InsertOrUpdateUser(user);

            // Act
            user.FullName = "Сидоров Сидорович";
            user.Role = "Учитель";
            _controller.InsertOrUpdateUser(user);

            // Assert
            var updatedUser = _controller.GetUserById(id);
            Assert.That(updatedUser.FullName, Is.EqualTo("Сидоров Сидорович"));
            Assert.That(updatedUser.Role, Is.EqualTo("Учитель"));
        }

        [Test]
        public void GetUserById_ExistingId_ReturnsUser()
        {
            // Arrange
            var testUser = new User { FullName = "Козлов Козлов", Role = "Учитель" };
            int id = _controller.InsertOrUpdateUser(testUser);

            // Act
            var foundUser = _controller.GetUserById(id);

            // Assert
            Assert.That(foundUser, Is.Not.Null);
            Assert.That(foundUser.UserID, Is.EqualTo(id));
            Assert.That(foundUser.FullName, Is.EqualTo("Козлов Козлов"));
        }

        [Test]
        public void GetUserById_NonExistingId_ReturnsNull()
        {
            var result = _controller.GetUserById(999);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void DeleteUser_ExistingUserWithoutDependencies_ReturnsTrue()
        {
            // Arrange
            var testUser = new User { FullName = "Тестовый", Role = "Учитель" };
            int id = _controller.InsertOrUpdateUser(testUser);

            // Act
            bool deleted = _controller.DeleteUser(testUser);

            // Assert
            Assert.That(deleted, Is.True);
            var deletedUser = _controller.GetUserById(id);
            Assert.That(deletedUser, Is.Null);
        }

        [Test]
        public void DeleteUser_WithHomeworkDependency_ThrowsInvalidOperationException()
        {
            // Arrange: Создаём пользователя + ДЗ
            var testUser = new User { FullName = "Учитель с ДЗ", Role = "Учитель" };
            int userId = _controller.InsertOrUpdateUser(testUser);

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                int classId = (int)new SqlCommand("INSERT INTO Classes (ClassName) OUTPUT INSERTED.ClassID VALUES ('10А')", conn).ExecuteScalar();
                int subjectId = (int)new SqlCommand("INSERT INTO Subjects (SubjectName) OUTPUT INSERTED.SubjectID VALUES ('Математика')", conn).ExecuteScalar();

                using (var cmd = new SqlCommand(@"
                    INSERT INTO Homework (AssignmentDate, ClassID, SubjectID, Description, TeacherID) 
                    VALUES ('2025-01-01', @ClassID, @SubjectID, 'Тест ДЗ', @TeacherID)", conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                    cmd.Parameters.AddWithValue("@TeacherID", userId);
                    cmd.ExecuteNonQuery();
                }
            }

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _controller.DeleteUser(testUser));
            StringAssert.Contains("Есть ДЗ (1)", ex.Message);
        }

        [Test]
        public void InsertOrUpdateUser_InvalidRole_ThrowsArgumentException()
        {
            var invalidUser = new User { FullName = "Тест", Role = "Админ" };
            Assert.Throws<ArgumentException>(() => _controller.InsertOrUpdateUser(invalidUser));
        }

        [Test]
        public void InsertOrUpdateUser_EmptyFullName_ThrowsArgumentException()
        {
            var invalidUser = new User { FullName = "", Role = "Учитель" };
            Assert.Throws<ArgumentException>(() => _controller.InsertOrUpdateUser(invalidUser));
        }

        [Test]
        public void GetUserByLoginPassword_ExistingUser_ReturnsUser()
        {
            // Arrange
            var testUser = new User { FullName = "ЛогинТест", Role = "Учитель" };
            _controller.InsertOrUpdateUser(testUser);

            // Act
            var foundUser = _controller.GetUserByLoginPassword("ЛогинТест", "anyPassword");

            // Assert
            Assert.That(foundUser, Is.Not.Null);
            Assert.That(foundUser.FullName, Is.EqualTo("ЛогинТест"));
        }

        [Test]
        public void GetUserByLoginPassword_NonExistingLogin_ReturnsNull()
        {
            var result = _controller.GetUserByLoginPassword("Несуществующий", "pass");
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetUserByLoginPassword_EmptyLogin_ReturnsNull()
        {
            var result = _controller.GetUserByLoginPassword("", "pass");
            Assert.That(result, Is.Null);
        }
    }
}
