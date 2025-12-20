using Microsoft.Data.SqlClient;
using NUnit.Framework;
using school.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school.Tests.Integration
{
    [TestFixture]
    public class SheduleControllerTests : Test
    {
        private string ConnectionString = Form1.CONNECTION_STRING;
        private SheduleController _controller;

        private const int TestClassId = 1;
        private const byte TestDayOfWeek = 1; 
        private const byte TestLessonNumber = 1;
        private const int TestSubjectId = 1;
        private const int TestTeacherId = 1;
        private ScheduleItem _testSchedule;

        [SetUp]
        public void SetUp()
        {
            _controller = new SheduleController();
            _testSchedule = new ScheduleItem
            {
                DayOfWeek = TestDayOfWeek,
                LessonNumber = TestLessonNumber,
                ClassID = TestClassId,
                SubjectID = TestSubjectId,
                TeacherID = TestTeacherId,
                LessonTime = TimeSpan.FromHours(8) // 08:00
            };

            // 1) ПОДГОТОВКА: очистка тестовой записи
            CleanupTestRecord();
        }

        [TearDown]
        public void TearDown()
        {
            // 3) УДАЛЕНИЕ ТЕСТОВЫХ ДАННЫХ
            CleanupTestRecord();
        }

        private void CleanupTestRecord()
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();

                SqlCommand cmd = new SqlCommand(@"
                    DELETE FROM Schedule 
                    WHERE DayOfWeek = @DayOfWeek 
                      AND LessonNumber = @LessonNumber 
                      AND ClassID = @ClassID", connection);

                cmd.Parameters.AddWithValue("@DayOfWeek", TestDayOfWeek);
                cmd.Parameters.AddWithValue("@LessonNumber", TestLessonNumber);
                cmd.Parameters.AddWithValue("@ClassID", TestClassId);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                connection?.Dispose();
            }
        }

        [Test]
        public void InsertOrUpdateSchedulePart_Inserts_New_Record()
        {
            // 2) ТЕСТ: ИНСТЕРТ
            int scheduleId = _controller.InsertOrUpdateSchedulePart(_testSchedule);

            // Проверка результата
            Assert.That(scheduleId, Is.GreaterThan(0), "Должен вернуть ID новой записи");

            // Проверка в БД
            VerifyScheduleInDatabase(scheduleId, _testSchedule);
        }

        [Test]
        public void InsertOrUpdateSchedulePart_Updates_Existing_Record()
        {
            // 1) Сначала INSERT
            int scheduleId = _controller.InsertOrUpdateSchedulePart(_testSchedule);

            // 2) Изменяем данные
            var updatedSchedule = new ScheduleItem
            {
                ScheduleID = scheduleId,
                DayOfWeek = TestDayOfWeek,
                LessonNumber = TestLessonNumber,
                ClassID = TestClassId,
                SubjectID = 2, // ✅ ИЗМЕНИЛИ SubjectID
                TeacherID = 2, // ✅ ИЗМЕНИЛИ TeacherID
                LessonTime = TimeSpan.FromHours(9) // ✅ ИЗМЕНИЛИ время
            };

            // 3) UPDATE
            int updatedId = _controller.InsertOrUpdateSchedulePart(updatedSchedule);

            // Проверка
            Assert.That(updatedId, Is.EqualTo(scheduleId), "Должен вернуть тот же ID");
            VerifyScheduleInDatabase(scheduleId, updatedSchedule);
        }

        [Test]
        public void InsertOrUpdateSchedulePart_Handles_Null_LessonTime()
        {
            var scheduleWithNullTime = new ScheduleItem
            {
                DayOfWeek = TestDayOfWeek,
                LessonNumber = TestLessonNumber + 1, // Другой урок
                ClassID = TestClassId,
                SubjectID = TestSubjectId,
                TeacherID = TestTeacherId,
                LessonTime = null // ✅ NULL время
            };

            int scheduleId = _controller.InsertOrUpdateSchedulePart(scheduleWithNullTime);

            Assert.That(scheduleId, Is.GreaterThan(0));
            VerifyScheduleInDatabase(scheduleId, scheduleWithNullTime);
        }

        private void VerifyScheduleInDatabase(int expectedId, ScheduleItem expected)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;
            try
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();

                SqlCommand cmd = new SqlCommand(@"
            SELECT ScheduleID, DayOfWeek, LessonNumber, ClassID, SubjectID, TeacherID, LessonTime
            FROM Schedule WHERE ScheduleID = @ScheduleID", connection);

                cmd.Parameters.AddWithValue("@ScheduleID", expectedId);
                reader = cmd.ExecuteReader();

                Assert.That(reader.Read(), Is.True, "Запись должна существовать в БД");

                int ordScheduleId = reader.GetOrdinal("ScheduleID");
                int ordDayOfWeek = reader.GetOrdinal("DayOfWeek");
                int ordLessonNumber = reader.GetOrdinal("LessonNumber");
                int ordClassId = reader.GetOrdinal("ClassID");
                int ordSubjectId = reader.GetOrdinal("SubjectID");
                int ordTeacherId = reader.GetOrdinal("TeacherID");
                int ordLessonTime = reader.GetOrdinal("LessonTime");

                Assert.That(reader.GetInt32(ordScheduleId), Is.EqualTo(expectedId));
                Assert.That(reader.GetByte(ordDayOfWeek), Is.EqualTo(expected.DayOfWeek));
                Assert.That(reader.GetByte(ordLessonNumber), Is.EqualTo(expected.LessonNumber));
                Assert.That(reader.GetInt32(ordClassId), Is.EqualTo(expected.ClassID));
                Assert.That(reader.GetInt32(ordSubjectId), Is.EqualTo(expected.SubjectID));
                Assert.That(reader.GetInt32(ordTeacherId), Is.EqualTo(expected.TeacherID));

                if (expected.LessonTime.HasValue)
                {
                    Assert.That(reader.IsDBNull(ordLessonTime), Is.False, "LessonTime не должен быть NULL");
                    Assert.That(reader.GetTimeSpan(ordLessonTime), Is.EqualTo(expected.LessonTime.Value));
                }
                else
                {
                    Assert.That(reader.IsDBNull(ordLessonTime), Is.True, "LessonTime должен быть NULL");
                }
            }
            finally
            {
                reader?.Dispose();
                connection?.Dispose();
            }
        }
    }
}
