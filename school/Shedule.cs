using Microsoft.Data.SqlClient;
using school.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school
{
    public class SheduleController
    {
        public static SheduleController _controller = new SheduleController();

        /// <summary>
        /// [translate:Получение полного расписания класса (вся неделя)]
        /// </summary>
        /// <param name="classId">ID класса</param>
        /// <returns>Все уроки класса на всю неделю</returns>
        public List<ScheduleItem> GetScheduleForClass(int classId)
        {
            var scheduleList = new List<ScheduleItem>();

            using (var conn = new SqlConnection(Form1.CONNECTION_STRING))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
            SELECT 
                s.ScheduleID, s.DayOfWeek, s.LessonNumber, s.LessonTime,
                s.ClassID, c.ClassName,
                s.SubjectID, sub.SubjectName,
                s.TeacherID, u.FullName AS TeacherName
            FROM Schedule s
            JOIN Classes c ON s.ClassID = c.ClassID
            JOIN Subjects sub ON s.SubjectID = sub.SubjectID
            JOIN Users u ON s.TeacherID = u.UserID
            WHERE s.ClassID = @ClassID
            ORDER BY s.DayOfWeek, s.LessonNumber", conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TimeSpan? lessonTime = null;
                            int timeOrdinal = reader.GetOrdinal("LessonTime");
                            if (!reader.IsDBNull(timeOrdinal))
                                lessonTime = reader.GetTimeSpan(timeOrdinal);

                            scheduleList.Add(new ScheduleItem
                            {
                                ScheduleID = reader.GetInt32(reader.GetOrdinal("ScheduleID")),
                                DayOfWeek = reader.GetByte(reader.GetOrdinal("DayOfWeek")),
                                LessonNumber = reader.GetByte(reader.GetOrdinal("LessonNumber")),
                                LessonTime = lessonTime,
                                ClassID = reader.GetInt32(reader.GetOrdinal("ClassID")),
                                ClassName = reader.GetString(reader.GetOrdinal("ClassName")),
                                SubjectID = reader.GetInt32(reader.GetOrdinal("SubjectID")),
                                SubjectName = reader.GetString(reader.GetOrdinal("SubjectName")),
                                TeacherID = reader.GetInt32(reader.GetOrdinal("TeacherID")),
                                TeacherName = reader.GetString(reader.GetOrdinal("TeacherName"))
                            });
                        }
                    }
                }
            }
            return scheduleList;
        }

        public List<ScheduleItem> GetScheduleForClassPeriod(int classId, DateTime startDate, DateTime endDate)
        {
            var scheduleList = new List<ScheduleItem>();

            using (var conn = new SqlConnection(Form1.CONNECTION_STRING))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
            SELECT 
                s.ScheduleID,
                s.DayOfWeek,
                s.LessonNumber,
                s.LessonTime,
                s.ClassID,
                c.ClassName,
                s.SubjectID,
                sub.SubjectName,
                s.TeacherID,
                u.FullName AS TeacherName
            FROM Schedule s
            JOIN Classes c ON s.ClassID = c.ClassID
            JOIN Subjects sub ON s.SubjectID = sub.SubjectID
            JOIN Users u ON s.TeacherID = u.UserID
            WHERE s.ClassID = @ClassID
                AND (
                    (@StartDate <= DATEFROMPARTS(YEAR(@StartDate), MONTH(@StartDate), 
                        CASE s.DayOfWeek 
                            WHEN 1 THEN 1  -- Пн
                            WHEN 2 THEN 2  -- Вт
                            WHEN 3 THEN 3  -- Ср
                            WHEN 4 THEN 4  -- Чт
                            WHEN 5 THEN 5  -- Пт
                            WHEN 6 THEN 6  -- Сб
                            WHEN 7 THEN 7  -- Вс
                        END)
                    AND DATEFROMPARTS(YEAR(@EndDate), MONTH(@EndDate), 
                        CASE s.DayOfWeek 
                            WHEN 1 THEN 1 WHEN 2 THEN 2 WHEN 3 THEN 3 
                            WHEN 4 THEN 4 WHEN 5 THEN 5 WHEN 6 THEN 6 WHEN 7 THEN 7
                        END) <= @EndDate)
                )
            ORDER BY s.DayOfWeek, s.LessonNumber", conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    cmd.Parameters.AddWithValue("@StartDate", startDate.Date);
                    cmd.Parameters.AddWithValue("@EndDate", endDate.Date);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TimeSpan? lessonTime = null;
                            int lessonTimeOrdinal = reader.GetOrdinal("LessonTime");
                            if (!reader.IsDBNull(lessonTimeOrdinal))
                                lessonTime = reader.GetTimeSpan(lessonTimeOrdinal);

                            scheduleList.Add(new ScheduleItem
                            {
                                ScheduleID = reader.GetInt32(reader.GetOrdinal("ScheduleID")),
                                DayOfWeek = reader.GetByte(reader.GetOrdinal("DayOfWeek")),
                                LessonNumber = reader.GetByte(reader.GetOrdinal("LessonNumber")),
                                LessonTime = lessonTime,
                                ClassID = reader.GetInt32(reader.GetOrdinal("ClassID")),
                                ClassName = reader.GetString(reader.GetOrdinal("ClassName")),
                                SubjectID = reader.GetInt32(reader.GetOrdinal("SubjectID")),
                                SubjectName = reader.GetString(reader.GetOrdinal("SubjectName")),
                                TeacherID = reader.GetInt32(reader.GetOrdinal("TeacherID")),
                                TeacherName = reader.GetString(reader.GetOrdinal("TeacherName"))
                            });
                        }

                    }
                }
            }
            return scheduleList;
        }

        /// <summary>
        /// Вставляет или обновляет урок в расписании (по DayOfWeek + LessonNumber + ClassID)
        /// </summary>
        /// <param name="schedule">Данные урока</param>
        /// <returns>ID вставленной/обновленной записи</returns>
        public int InsertOrUpdateSchedulePart(ScheduleItem schedule)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(Form1.CONNECTION_STRING);
                connection.Open();

                // ✅ Проверяем существование по ключу (DayOfWeek + LessonNumber + ClassID)
                SqlCommand checkCmd = new SqlCommand(@"
            SELECT ScheduleID FROM Schedule 
            WHERE DayOfWeek = @DayOfWeek AND LessonNumber = @LessonNumber AND ClassID = @ClassID", connection);

                checkCmd.Parameters.AddWithValue("@DayOfWeek", schedule.DayOfWeek);
                checkCmd.Parameters.AddWithValue("@LessonNumber", schedule.LessonNumber);
                checkCmd.Parameters.AddWithValue("@ClassID", schedule.ClassID);

                object existingId = checkCmd.ExecuteScalar();

                if (existingId != null)
                {
                    // ✅ ОБНОВИТЬ существующий
                    SqlCommand updateCmd = new SqlCommand(@"
                UPDATE Schedule SET 
                    SubjectID = @SubjectID, 
                    TeacherID = @TeacherID,
                    LessonTime = @LessonTime
                WHERE ScheduleID = @ScheduleID", connection);

                    updateCmd.Parameters.AddWithValue("@SubjectID", schedule.SubjectID);
                    updateCmd.Parameters.AddWithValue("@TeacherID", schedule.TeacherID);
                    updateCmd.Parameters.AddWithValue("@LessonTime",
                        schedule.LessonTime.HasValue ? (object)schedule.LessonTime.Value : DBNull.Value);
                    updateCmd.Parameters.AddWithValue("@ScheduleID", (int)existingId);

                    updateCmd.ExecuteNonQuery();
                    return (int)existingId;
                }
                else
                {
                    // ✅ НОВАЯ СТРОКА: ScheduleID — identity, генерируется БД
                    SqlCommand insertCmd = new SqlCommand(@"
                INSERT INTO Schedule (DayOfWeek, LessonNumber, ClassID, SubjectID, TeacherID, LessonTime)
                OUTPUT INSERTED.ScheduleID
                VALUES (@DayOfWeek, @LessonNumber, @ClassID, @SubjectID, @TeacherID, @LessonTime)", connection);

                    insertCmd.Parameters.AddWithValue("@DayOfWeek", schedule.DayOfWeek);
                    insertCmd.Parameters.AddWithValue("@LessonNumber", schedule.LessonNumber);
                    insertCmd.Parameters.AddWithValue("@ClassID", schedule.ClassID);
                    insertCmd.Parameters.AddWithValue("@SubjectID", schedule.SubjectID);
                    insertCmd.Parameters.AddWithValue("@TeacherID", schedule.TeacherID);
                    insertCmd.Parameters.AddWithValue("@LessonTime",
                        schedule.LessonTime.HasValue ? (object)schedule.LessonTime.Value : DBNull.Value);

                    int newScheduleId = (int)insertCmd.ExecuteScalar();
                    return newScheduleId; // ✅ Возвращаем ID из identity
                }
            }
            finally
            {
                connection?.Dispose();
            }
        }

        /// <summary>
        /// Удаляет урок расписания по ScheduleID
        /// </summary>
        public void DeleteSchedulePartById(int scheduleId)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(Form1.CONNECTION_STRING);
                connection.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM Schedule WHERE ScheduleID = @ScheduleID", connection);
                cmd.Parameters.AddWithValue("@ScheduleID", scheduleId);

                int deletedRows = cmd.ExecuteNonQuery();
            }
            finally
            {
                connection?.Dispose();
            }
        }
    }
}
