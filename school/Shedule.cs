using Microsoft.Data.SqlClient;
using school.Controllers;
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
        public class ScheduleChange
        {
            public string Action { get; set; } // "EDIT", "ADD", "DELETE"
            public ScheduleItem Schedule { get; set; } = new ScheduleItem();
        }

        public static SheduleController _controller = new SheduleController();

        // ✅ КОЛЛЕКЦИЯ ИЗМЕНЕНИЙ РАСПИСАНИЯ
        private List<ScheduleChange> pendingChanges = new List<ScheduleChange>();

        /// <summary>
        /// Добавляет изменение расписания в очередь
        /// </summary>
        public void AddScheduleChange(string action, ScheduleItem schedule)
        {
            if (UserController.CurrentUser.PermissionID < 3) return; // Только директор

            pendingChanges.Add(new ScheduleChange
            {
                Action = action,
                Schedule = schedule
            });
        }

        /// <summary>
        /// Выполняет все изменения расписания и очищает очередь
        /// </summary>
        public int CommitScheduleChanges()
        {
            if (pendingChanges.Count == 0) return 0;

            int processed = 0;
            try
            {
                foreach (var change in pendingChanges)
                {
                    switch (change.Action.ToUpper())
                    {
                        case "EDIT":
                        case "ADD":
                            int resultId = InsertOrUpdateSchedulePart(change.Schedule);
                            change.Schedule.ScheduleID = resultId;
                            processed++;
                            break;
                        case "DELETE":
                            if (change.Schedule.ScheduleID > 0)
                            {
                                DeleteSchedulePartById(change.Schedule.ScheduleID);
                                processed++;
                            }
                            break;
                    }
                }
                pendingChanges.Clear();
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"Commit schedule changes error: {ex.Message}", ex);
            }
            return processed;
        }

        /// <summary>
        /// Количество ожидающих изменений расписания
        /// </summary>
        public int PendingChangesCount => pendingChanges.Count;

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

                FileLogger.logger.Info($"InsertOrUpdateSchedulePart: ScheduleID={schedule.ScheduleID}, Day={schedule.DayOfWeek}, Lesson={schedule.LessonNumber}, Class={schedule.ClassID}");

                if (schedule.ScheduleID == -1 || schedule.ScheduleID == 0)
                {
                    // ✅ НОВЫЙ урок - INSERT с IDENTITY
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

                    int newId = (int)insertCmd.ExecuteScalar();
                    FileLogger.logger.Info($"INSERT success: new ScheduleID={newId}");
                    return newId;
                }
                else
                {
                    // ✅ Проверяем существование записи
                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Schedule WHERE ScheduleID = @ScheduleID", connection);
                    checkCmd.Parameters.AddWithValue("@ScheduleID", schedule.ScheduleID);
                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists == 0)
                    {
                        FileLogger.logger.Warn($"UPDATE failed: ScheduleID={schedule.ScheduleID} not found. Doing INSERT instead.");
                        // Fallback на INSERT
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

                        int newId = (int)insertCmd.ExecuteScalar();
                        return newId;
                    }

                    // ✅ UPDATE по ID
                    SqlCommand updateCmd = new SqlCommand(@"
                    UPDATE Schedule SET 
                        DayOfWeek = @DayOfWeek,
                        LessonNumber = @LessonNumber,
                        SubjectID = @SubjectID, 
                        TeacherID = @TeacherID,
                        LessonTime = @LessonTime
                    WHERE ScheduleID = @ScheduleID", connection);

                    updateCmd.Parameters.AddWithValue("@DayOfWeek", schedule.DayOfWeek);
                    updateCmd.Parameters.AddWithValue("@LessonNumber", schedule.LessonNumber);
                    updateCmd.Parameters.AddWithValue("@ScheduleID", schedule.ScheduleID);
                    updateCmd.Parameters.AddWithValue("@SubjectID", schedule.SubjectID);
                    updateCmd.Parameters.AddWithValue("@TeacherID", schedule.TeacherID);
                    updateCmd.Parameters.AddWithValue("@LessonTime",
                        schedule.LessonTime.HasValue ? (object)schedule.LessonTime.Value : DBNull.Value);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    FileLogger.logger.Info($"UPDATE success: ScheduleID={schedule.ScheduleID}, rows={rowsAffected}");

                    if (rowsAffected == 0)
                        FileLogger.logger.Error($"UPDATE affected 0 rows for ScheduleID={schedule.ScheduleID}");

                    return schedule.ScheduleID;
                }
            }
            catch (SqlException ex)
            {
                FileLogger.logger.Error($"SQL Error in InsertOrUpdateSchedulePart: {ex.Message}", ex);
                throw;
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"Error in InsertOrUpdateSchedulePart: {ex.Message}", ex);
                throw;
            }
            finally
            {
                connection?.Dispose();
            }
        }


        /// <summary>
        /// Возвращает расписание для конкретного учителя/директора.
        /// Директор (PermissionID = 3) видит ВСЁ расписание.
        /// Учитель видит только свои уроки.
        /// </summary>
        /// <param name="teacherId">ID учителя/директора из Users.UserID</param>
        /// <returns>Список ScheduleItem с полными данными</returns>
        public List<ScheduleItem> GetScheduleForTeacher(int teacherId)
        {
            var result = new List<ScheduleItem>();

            using (var connection = new SqlConnection(Form1.CONNECTION_STRING))
            {
                connection.Open();

                // 1. Получаем PermissionID пользователя (учитель=2, директор=3)
                int permissionId = 0;
                using (var cmdPerm = new SqlCommand(
                    "SELECT PermissionID FROM Users WHERE UserID = @UserID", connection))
                {
                    cmdPerm.Parameters.AddWithValue("@UserID", teacherId);
                    var permObj = cmdPerm.ExecuteScalar();
                    permissionId = permObj == DBNull.Value || permObj == null ? 0 : Convert.ToInt32(permObj);
                }

                string sql;
                SqlCommand cmd;

                if (permissionId == 3) // Директор видит ВСЁ
                {
                    sql = @"
                SELECT 
                    s.ScheduleID, s.DayOfWeek, s.LessonNumber, s.ClassID, s.SubjectID, s.TeacherID, s.LessonTime,
                    c.ClassName,
                    sub.SubjectName,
                    u.FullName AS TeacherName
                FROM Schedule s
                INNER JOIN Classes c ON s.ClassID = c.ClassID
                INNER JOIN Subjects sub ON s.SubjectID = sub.SubjectID
                INNER JOIN Users u ON s.TeacherID = u.UserID
                ORDER BY c.ClassName, s.DayOfWeek, s.LessonNumber";

                    cmd = new SqlCommand(sql, connection);
                }
                else // Учитель видит только свои уроки
                {
                    sql = @"
                SELECT 
                    s.ScheduleID, s.DayOfWeek, s.LessonNumber, s.ClassID, s.SubjectID, s.TeacherID, s.LessonTime,
                    c.ClassName,
                    sub.SubjectName,
                    u.FullName AS TeacherName
                FROM Schedule s
                INNER JOIN Classes c ON s.ClassID = c.ClassID
                INNER JOIN Subjects sub ON s.SubjectID = sub.SubjectID
                INNER JOIN Users u ON s.TeacherID = u.UserID
                WHERE s.TeacherID = @TeacherID
                ORDER BY s.DayOfWeek, s.LessonNumber, c.ClassName";

                    cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                }

                using (cmd)
                using (var reader = cmd.ExecuteReader())
                {
                    // Получаем ordinals один раз для производительности и безопасности
                    int ordScheduleID = reader.GetOrdinal("ScheduleID");
                    int ordDayOfWeek = reader.GetOrdinal("DayOfWeek");
                    int ordLessonNumber = reader.GetOrdinal("LessonNumber");
                    int ordClassID = reader.GetOrdinal("ClassID");
                    int ordClassName = reader.GetOrdinal("ClassName");
                    int ordSubjectID = reader.GetOrdinal("SubjectID");
                    int ordSubjectName = reader.GetOrdinal("SubjectName");
                    int ordTeacherID = reader.GetOrdinal("TeacherID");
                    int ordTeacherName = reader.GetOrdinal("TeacherName");
                    int ordLessonTime = reader.GetOrdinal("LessonTime");

                    while (reader.Read())
                    {
                        try
                        {
                            var item = new ScheduleItem
                            {
                                ScheduleID = reader.IsDBNull(ordScheduleID) ? 0 : reader.GetInt32(ordScheduleID),
                                DayOfWeek = reader.IsDBNull(ordDayOfWeek) ? (byte)1 : reader.GetByte(ordDayOfWeek),
                                LessonNumber = reader.IsDBNull(ordLessonNumber) ? (byte)1 : reader.GetByte(ordLessonNumber),
                                LessonTime = reader.IsDBNull(ordLessonTime) ? (TimeSpan?)null : reader.GetTimeSpan(ordLessonTime),
                                ClassID = reader.IsDBNull(ordClassID) ? 0 : reader.GetInt32(ordClassID),
                                ClassName = reader.IsDBNull(ordClassName) ? "" : reader.GetString(ordClassName),
                                SubjectID = reader.IsDBNull(ordSubjectID) ? 0 : reader.GetInt32(ordSubjectID),
                                SubjectName = reader.IsDBNull(ordSubjectName) ? "" : reader.GetString(ordSubjectName),
                                TeacherID = reader.IsDBNull(ordTeacherID) ? 0 : reader.GetInt32(ordTeacherID),
                                TeacherName = reader.IsDBNull(ordTeacherName) ? "" : reader.GetString(ordTeacherName)
                            };

                            result.Add(item);
                        }
                        catch (Exception exRow)
                        {
                            // Логирование ошибки чтения строки, но не прерываем весь цикл
                            FileLogger.logger.Error("GetScheduleForTeacher: ошибка парсинга записи расписания", exRow);
                        }
                    }
                }
            }

            return result;
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
