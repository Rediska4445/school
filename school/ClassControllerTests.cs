using NUnit.Framework;
using school.Controllers;
using school.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using System;

namespace school.Tests.Integration
{
    [TestFixture]
    public class ClassControllerTests
    {
        private ClassController _controller;
        private string _connectionString = Form1.CONNECTION_STRING;

        [SetUp]
        public void Setup()
        {
            _controller = new ClassController(null);

            // Создаём тестовую БД или очищаем
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
                using (var cmd = new SqlCommand("DELETE FROM Classes", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [Test]
        public void InsertOrUpdateClass_NewClass_ReturnsNewId()
        {
            // Arrange
            var newClass = new Class { ClassName = "10А" };

            // Act
            int resultId = _controller.InsertOrUpdateClass(newClass);

            // Assert
            Assert.That(resultId, Is.GreaterThan(0));
            Assert.That(newClass.ClassID, Is.EqualTo(resultId));

            // Проверяем в БД
            var savedClass = _controller.GetClassById(resultId);
            Assert.That(savedClass, Is.Not.Null);
            Assert.That(savedClass.ClassName, Is.EqualTo("10А"));
        }

        [Test]
        public void InsertOrUpdateClass_UpdateExistingClass_UpdatesName()
        {
            // Arrange
            var newClass = new Class { ClassName = "10А" };
            int id = _controller.InsertOrUpdateClass(newClass);

            // Act
            newClass.ClassName = "10Б";
            newClass.ClassID = id;
            _controller.InsertOrUpdateClass(newClass);

            // Assert
            var updatedClass = _controller.GetClassById(id);
            Assert.That(updatedClass.ClassName, Is.EqualTo("10Б"));
        }

        [Test]
        public void GetClassById_ExistingId_ReturnsClass()
        {
            // Arrange
            var testClass = new Class { ClassName = "11В" };
            int id = _controller.InsertOrUpdateClass(testClass);

            // Act
            var foundClass = _controller.GetClassById(id);

            // Assert
            Assert.That(foundClass, Is.Not.Null);
            Assert.That(foundClass.ClassID, Is.EqualTo(id));
            Assert.That(foundClass.ClassName, Is.EqualTo("11В"));
        }

        [Test]
        public void GetClassById_NonExistingId_ReturnsNull()
        {
            // Act
            var result = _controller.GetClassById(999);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void DeleteClass_ExistingClass_ReturnsTrue()
        {
            // Arrange
            var testClass = new Class { ClassName = "12Г" };
            int id = _controller.InsertOrUpdateClass(testClass);

            // Act
            bool deleted = _controller.DeleteClass(testClass);

            // Assert
            Assert.That(deleted, Is.True);
            var deletedClass = _controller.GetClassById(id);
            Assert.That(deletedClass, Is.Null);
        }

        [Test]
        public void DeleteClass_NonExistingClass_ReturnsFalse()
        {
            // Arrange
            var nonExistingClass = new Class { ClassID = 999 };

            // Act
            bool deleted = _controller.DeleteClass(nonExistingClass);

            // Assert
            Assert.That(deleted, Is.False);
        }

        [Test]
        public void InsertOrUpdateClass_InvalidClass_ThrowsArgumentException()
        {
            // Arrange
            var invalidClass = new Class { ClassName = "" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _controller.InsertOrUpdateClass(invalidClass));
        }

        [Test]
        public void InsertOrUpdateClass_NullClass_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _controller.InsertOrUpdateClass(null));
        }
    }
}
