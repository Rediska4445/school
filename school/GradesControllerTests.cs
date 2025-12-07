using System;
using Microsoft.Data.SqlClient;
using school.Models;
using System.Linq;
using NUnit.Framework;

namespace school.Tests.Integration
{
    [TestFixture]
    public class GradesControllerTests
    {
        private string _connectionString;
        private GradesController _controller;
        private int _testStudentId;
        private int _testSubjectId;
        private int _testTeacherId;
        private int _testClassId;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _connectionString = Form1.CONNECTION_STRING;
        }

        [SetUp]
        public void SetUp()
        {
            _controller = new GradesController(_connectionString);
            CreateTestData();
        }

        [TearDown]
        public void TearDown()
        {
            CleanupTestData();
        }


        private void CreateTestData()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Создаем класс
                SqlCommand classCmd = new SqlCommand("INSERT INTO Classes (ClassName) VALUES (N'10А'); SELECT SCOPE_IDENTITY();", conn);
                _testClassId = Convert.ToInt32(classCmd.ExecuteScalar());

                // Ученик
                SqlCommand studentCmd = new SqlCommand(
                    "INSERT INTO Users (FullName, Role, ClassID) VALUES (N'Иванов Иван', N'Ученик', @ClassID); SELECT SCOPE_IDENTITY();", conn);
                studentCmd.Parameters.AddWithValue("@ClassID", _testClassId);
                _testStudentId = Convert.ToInt32(studentCmd.ExecuteScalar());

                // Предмет
                SqlCommand subjectCmd = new SqlCommand("INSERT INTO Subjects (SubjectName) VALUES (N'Математика'); SELECT SCOPE_IDENTITY();", conn);
                _testSubjectId = Convert.ToInt32(subjectCmd.ExecuteScalar());

                // Учитель
                SqlCommand teacherCmd = new SqlCommand("INSERT INTO Users (FullName, Role) VALUES (N'Петров Петр', N'Учитель'); SELECT SCOPE_IDENTITY();", conn);
                _testTeacherId = Convert.ToInt32(teacherCmd.ExecuteScalar());
            }
        }

        private void CleanupTestData()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Удаляем оценки
                SqlCommand deleteGradesCmd = new SqlCommand("DELETE FROM Grades WHERE StudentID = @StudentID OR TeacherID = @TeacherID", conn);
                deleteGradesCmd.Parameters.AddWithValue("@StudentID", _testStudentId);
                deleteGradesCmd.Parameters.AddWithValue("@TeacherID", _testTeacherId);
                deleteGradesCmd.ExecuteNonQuery();

                // Удаляем пользователей
                SqlCommand deleteUsersCmd = new SqlCommand("DELETE FROM Users WHERE UserID = @StudentID OR UserID = @TeacherID", conn);
                deleteUsersCmd.Parameters.AddWithValue("@StudentID", _testStudentId);
                deleteUsersCmd.Parameters.AddWithValue("@TeacherID", _testTeacherId);
                deleteUsersCmd.ExecuteNonQuery();

                // Удаляем предмет
                SqlCommand deleteSubjectCmd = new SqlCommand("DELETE FROM Subjects WHERE SubjectID = @SubjectID", conn);
                deleteSubjectCmd.Parameters.AddWithValue("@SubjectID", _testSubjectId);
                deleteSubjectCmd.ExecuteNonQuery();

                // Удаляем класс
                SqlCommand deleteClassCmd = new SqlCommand("DELETE FROM Classes WHERE ClassID = @ClassID", conn);
                deleteClassCmd.Parameters.AddWithValue("@ClassID", _testClassId);
                deleteClassCmd.ExecuteNonQuery();
            }
        }

        [Test]
        public void InsertOrUpdateGrade_NewGrade_InsertsSuccessfully()
        {
            // Arrange
            Grade grade = new Grade
            {
                GradeDate = new DateTime(2025, 12, 7),
                StudentID = _testStudentId,
                SubjectID = _testSubjectId,
                GradeValue = 5,
                TeacherID = _testTeacherId
            };

            // Act
            _controller.InsertOrUpdateGrade(grade);

            // Assert
            Grade savedGrade = _controller.GetGradeBySubjectStudentDate(
                new Subject { SubjectID = _testSubjectId },
                new User { UserID = _testStudentId },
                grade.GradeDate);

            Assert.That(savedGrade, Is.Not.Null);
            Assert.That(savedGrade.GradeValue, Is.EqualTo(grade.GradeValue));
            Assert.That(savedGrade.TeacherID, Is.EqualTo(grade.TeacherID));
        }

        [Test]
        public void InsertOrUpdateGrade_ExistingGrade_UpdatesSuccessfully()
        {
            // Arrange
            Grade grade = new Grade
            {
                GradeDate = new DateTime(2025, 12, 7),
                StudentID = _testStudentId,
                SubjectID = _testSubjectId,
                GradeValue = 3,
                TeacherID = _testTeacherId
            };

            _controller.InsertOrUpdateGrade(grade); // Вставка

            // Act
            grade.GradeValue = 5;
            _controller.InsertOrUpdateGrade(grade);

            // Assert
            Grade savedGrade = _controller.GetGradeBySubjectStudentDate(
                new Subject { SubjectID = _testSubjectId },
                new User { UserID = _testStudentId },
                grade.GradeDate);

            Assert.That(savedGrade.GradeValue, Is.EqualTo(5));
        }

        [Test]
        public void DeleteGrade_ExistingGrade_RemovesSuccessfully()
        {
            // Arrange
            Grade grade = new Grade
            {
                GradeDate = new DateTime(2025, 12, 7),
                StudentID = _testStudentId,
                SubjectID = _testSubjectId,
                GradeValue = 4,
                TeacherID = _testTeacherId
            };

            _controller.InsertOrUpdateGrade(grade);
            Grade savedGrade = _controller.GetGradeBySubjectStudentDate(
                new Subject { SubjectID = _testSubjectId },
                new User { UserID = _testStudentId },
                grade.GradeDate);
            grade.GradeID = savedGrade.GradeID;

            // Act
            _controller.DeleteGrade(grade);

            // Assert
            Grade deletedGrade = _controller.GetGradeBySubjectStudentDate(
                new Subject { SubjectID = _testSubjectId },
                new User { UserID = _testStudentId },
                grade.GradeDate);

            Assert.That(deletedGrade, Is.Null);
        }

        [Test]
        public void GetGradeBySubjectStudentDate_Existing_ReturnsGrade()
        {
            // Arrange
            Grade grade = new Grade
            {
                GradeDate = new DateTime(2025, 12, 7),
                StudentID = _testStudentId,
                SubjectID = _testSubjectId,
                GradeValue = 4,
                TeacherID = _testTeacherId
            };
            _controller.InsertOrUpdateGrade(grade);

            // Act
            Grade result = _controller.GetGradeBySubjectStudentDate(
                new Subject { SubjectID = _testSubjectId },
                new User { UserID = _testStudentId },
                grade.GradeDate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GradeValue, Is.EqualTo(4));
        }

        [Test]
        public void GetGradeBySubjectNameStudentLoginDate_Existing_ReturnsGrade()
        {
            // Arrange
            Grade grade = new Grade
            {
                GradeDate = new DateTime(2025, 12, 8),
                StudentID = _testStudentId,
                SubjectID = _testSubjectId,
                GradeValue = 5,
                TeacherID = _testTeacherId
            };
            _controller.InsertOrUpdateGrade(grade);

            // Act
            Grade result = _controller.GetGradeBySubjectNameStudentLoginDate("Математика", "Иванов Иван", grade.GradeDate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GradeValue, Is.EqualTo(5));
        }

        [Test]
        public void GetGradeBySubjectNameStudentLoginDate_NotFound_ReturnsNull()
        {
            // Act & Assert
            Grade result = _controller.GetGradeBySubjectNameStudentLoginDate("Физика", "Сидоров Сидор", new DateTime(2025, 12, 1));
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetGradesForPeriod_WithGrades_ReturnsCorrectList()
        {
            // Arrange
            _controller.InsertOrUpdateGrade(new Grade
            {
                GradeDate = new DateTime(2025, 12, 7),
                StudentID = _testStudentId,
                SubjectID = _testSubjectId,
                GradeValue = 4,
                TeacherID = _testTeacherId
            });

            // Act
            var grades = _controller.GetGradesForPeriod(new DateTime(2025, 12, 1), new DateTime(2025, 12, 31));

            // Assert
            Assert.That(grades.Count, Is.EqualTo(1));
            Assert.That(grades[0].GradeValue, Is.EqualTo(4));
            Assert.That(grades[0].StudentID, Is.EqualTo(_testStudentId));
        }

        [Test]
        public void GetGradesForPeriod_EmptyPeriod_ReturnsEmptyList()
        {
            // Act
            var grades = _controller.GetGradesForPeriod(new DateTime(2025, 1, 1), new DateTime(2025, 1, 31));

            // Assert
            Assert.That(grades.Count, Is.EqualTo(0));
        }
    }
}
