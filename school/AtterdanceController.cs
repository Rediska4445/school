using Microsoft.Data.SqlClient;
using school.Controllers;
using school.Models;
using System;
using System.Collections.Generic;

namespace school
{
    public class AtterdanceController
    {
        public static AtterdanceController Instance = new AtterdanceController();

        public class AttendanceChange
        {
            public string Action { get; set; } // "EDIT", "ADD", "DELETE"
            public Attendance Attendance { get; set; } = new Attendance();
            public int OriginalAttendanceID { get; set; } = 0; // Для новых строк
        }

        private List<AttendanceChange> pendingChanges = new List<AttendanceChange>();

        /// <summary>
        /// Добавляет изменение посещаемости в очередь
        /// </summary>
        public void AddAttendanceChange(string action, Attendance attendance)
        {
            if (UserController.CurrentUser.PermissionID <= 1) return; // Только учителя

            pendingChanges.Add(new AttendanceChange
            {
                Action = action.ToUpper(),
                Attendance = attendance
            });

            FileLogger.logger.Info($"📝 Добавлено изменение посещаемости: {action} (ID: {attendance.AttendanceID})");
        }

        /// <summary>
        /// Выполняет все изменения посещаемости и очищает очередь
        /// </summary>
        public int CommitAttendanceChanges()
        {
            if (pendingChanges.Count == 0) return 0;

            int processed = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(Form1.CONNECTION_STRING))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (var change in pendingChanges)
                            {
                                switch (change.Action)
                                {
                                    case "EDIT":
                                    case "ADD":
                                        InsertOrUpdateAttendance(change.Attendance);
                                        processed++;
                                        break;
                                    case "DELETE":
                                        DeleteAttendance(change.Attendance.AttendanceID);
                                        processed++;
                                        break;
                                }
                            }
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                pendingChanges.Clear();
                FileLogger.logger.Info($"✅ Сохранено {processed} изменений посещаемости");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"❌ Ошибка сохранения: {ex.Message}");
                throw;
            }
            return processed;
        }

        /// <summary>
        /// Количество ожидающих изменений посещаемости
        /// </summary>
        public int PendingChangesCount => pendingChanges.Count;

        public void DeleteAttendance(int attendanceId)
        {
            if (attendanceId <= 0)
            {
                FileLogger.logger.Warn("Попытка удалить несуществующую запись посещаемости (ID <= 0)");
                return;
            }

            using (SqlConnection conn = new SqlConnection(Form1.CONNECTION_STRING))
            {
                conn.Open();

                string query = @"
                    DELETE FROM Attendance 
                    WHERE AttendanceID = @AttendanceID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AttendanceID", attendanceId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        FileLogger.logger.Info($"✅ Удалена запись посещаемости ID={attendanceId}");
                    }
                    else
                    {
                        FileLogger.logger.Warn($"⚠️ Запись посещаемости ID={attendanceId} не найдена");
                    }
                }
            }
        }

        private int InsertOrUpdateAttendance(Attendance attendance)
        {
            using (SqlConnection conn = new SqlConnection(Form1.CONNECTION_STRING))
            {
                conn.Open();

                if (attendance.AttendanceID < 0) // ✅ НОВАЯ запись - всегда INSERT
                {
                    string insertQuery = @"
                INSERT INTO Attendance (AttendanceDate, UserID, SubjectID, Present, ExcuseReason, LessonDate, Comment)
                OUTPUT INSERTED.AttendanceID
                VALUES (@AttendanceDate, @UserID, @SubjectID, @Present, @ExcuseReason, @LessonDate, @Comment)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        AddAttendanceParams(cmd, attendance);
                        int newId = (int)cmd.ExecuteScalar();
                        attendance.AttendanceID = newId; // Обновляем объект
                        return newId;
                    }
                }
                else // ID >= 0 - проверяем по AttendanceID
                {
                    // 1. Проверяем существование по AttendanceID
                    using (var checkCmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Attendance WHERE AttendanceID = @AttendanceID", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@AttendanceID", attendance.AttendanceID);
                        bool exists = (int)checkCmd.ExecuteScalar() > 0;

                        if (exists)
                        {
                            // 2. UPDATE существующей
                            string updateQuery = @"
                        UPDATE Attendance SET 
                            AttendanceDate = @AttendanceDate,
                            UserID = @UserID,
                            SubjectID = @SubjectID,
                            Present = @Present,
                            ExcuseReason = @ExcuseReason,
                            LessonDate = @LessonDate,
                            Comment = @Comment
                        WHERE AttendanceID = @AttendanceID";

                            using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                            {
                                AddAttendanceParams(cmd, attendance);
                                cmd.Parameters.AddWithValue("@AttendanceID", attendance.AttendanceID);
                                cmd.ExecuteNonQuery();
                                return attendance.AttendanceID;
                            }
                        }
                        else
                        {
                            // 3. INSERT новой (ID был "фальшивым")
                            string insertQuery = @"
                        INSERT INTO Attendance (AttendanceDate, UserID, SubjectID, Present, ExcuseReason, LessonDate, Comment)
                        OUTPUT INSERTED.AttendanceID
                        VALUES (@AttendanceDate, @UserID, @SubjectID, @Present, @ExcuseReason, @LessonDate, @Comment)";

                            using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                            {
                                AddAttendanceParams(cmd, attendance);
                                int newId = (int)cmd.ExecuteScalar();
                                return newId;
                            }
                        }
                    }
                }
            }
        }

        private void AddAttendanceParams(SqlCommand cmd, Attendance attendance)
        {
            cmd.Parameters.AddWithValue("@AttendanceDate", attendance.AttendanceDate.Date);
            cmd.Parameters.AddWithValue("@UserID", attendance.UserID);
            cmd.Parameters.AddWithValue("@SubjectID", attendance.SubjectID);
            cmd.Parameters.AddWithValue("@Present", attendance.Present);
            cmd.Parameters.AddWithValue("@ExcuseReason", attendance.ExcuseReason);
            cmd.Parameters.AddWithValue("@LessonDate",
                attendance.LessonDate == DateTime.MinValue ? (object)DBNull.Value : attendance.LessonDate);
            cmd.Parameters.AddWithValue("@Comment",
                string.IsNullOrEmpty(attendance.Comment) ? (object)DBNull.Value : attendance.Comment);
        }

        public List<Attendance> GetClassAttendance(int classId, DateTime startDate, DateTime endDate)
        {
            List<Attendance> attendance = new List<Attendance>();

            string query = @"
                SELECT 
                    a.AttendanceID,
                    a.AttendanceDate,
                    a.UserID,
                    u.FullName,
                    u.UserID as StudentUserID,  -- для User
                    a.Present,
                    a.ExcuseReason,
                    a.LessonDate,
                    a.Comment,
                    ISNULL(a.SubjectID, 0) AS SubjectID,
                    ISNULL(s.SubjectName, N'') AS SubjectName,
                    CASE 
                        WHEN a.Present = 1 THEN N'Присутствовал'
                        WHEN a.ExcuseReason = 1 THEN N'Уважительная'
                        ELSE N'Отсутствовал'
                    END AS StatusDisplay
                FROM Attendance a
                INNER JOIN Users u ON a.UserID = u.UserID
                LEFT JOIN Subjects s ON a.SubjectID = s.SubjectID
                WHERE u.ClassID = @ClassID 
                    AND a.AttendanceDate BETWEEN @StartDate AND @EndDate
                ORDER BY a.AttendanceDate DESC, u.FullName";

            using (SqlConnection conn = new SqlConnection(Form1.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    cmd.Parameters.AddWithValue("@StartDate", startDate.Date);
                    cmd.Parameters.AddWithValue("@EndDate", endDate.Date);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int idxAttendanceID = reader.GetOrdinal("AttendanceID");
                        int idxAttendanceDate = reader.GetOrdinal("AttendanceDate");
                        int idxUserID = reader.GetOrdinal("UserID");
                        int idxStudentUserID = reader.GetOrdinal("StudentUserID");
                        int idxFullName = reader.GetOrdinal("FullName");
                        int idxPresent = reader.GetOrdinal("Present");
                        int idxExcuseReason = reader.GetOrdinal("ExcuseReason");
                        int idxLessonDate = reader.GetOrdinal("LessonDate");
                        int idxComment = reader.GetOrdinal("Comment");
                        int idxSubjectID = reader.GetOrdinal("SubjectID");
                        int idxSubjectName = reader.GetOrdinal("SubjectName");
                        int idxStatusDisplay = reader.GetOrdinal("StatusDisplay");

                        while (reader.Read())
                        {
                            User student = new User
                            {
                                UserID = !reader.IsDBNull(idxStudentUserID) ? reader.GetInt32(idxStudentUserID) : 0,
                                FullName = !reader.IsDBNull(idxFullName) ? reader.GetString(idxFullName) : ""
                            };

                            attendance.Add(new Attendance
                            {
                                AttendanceID = !reader.IsDBNull(idxAttendanceID) ? reader.GetInt32(idxAttendanceID) : 0,
                                AttendanceDate = !reader.IsDBNull(idxAttendanceDate) ? reader.GetDateTime(idxAttendanceDate) : DateTime.Today,
                                UserID = !reader.IsDBNull(idxUserID) ? reader.GetInt32(idxUserID) : 0,
                                Student = student,
                                Present = !reader.IsDBNull(idxPresent) ? reader.GetBoolean(idxPresent) : true,
                                ExcuseReason = !reader.IsDBNull(idxExcuseReason) ? reader.GetBoolean(idxExcuseReason) : false,
                                LessonDate = !reader.IsDBNull(idxLessonDate) ? reader.GetDateTime(idxLessonDate) : DateTime.MinValue,
                                Comment = !reader.IsDBNull(idxComment) ? reader.GetString(idxComment) : "",
                                SubjectID = !reader.IsDBNull(idxSubjectID) ? reader.GetInt32(idxSubjectID) : 0,
                                SubjectName = !reader.IsDBNull(idxSubjectName) ? reader.GetString(idxSubjectName) : "",
                                StatusDisplay = !reader.IsDBNull(idxStatusDisplay) ? reader.GetString(idxStatusDisplay) : ""
                            });
                        }
                    }
                }
            }

            return attendance;
        }

        public List<Attendance> GetStudentAttendance(int userId)
        {
            using (var conn = new SqlConnection(Form1.CONNECTION_STRING))
            {
                conn.Open();

                using (var cmd = new SqlCommand(@"
                    SELECT 
                        a.AttendanceID,           -- 0
                        a.AttendanceDate,         -- 1  
                        a.LessonDate,             -- 2
                        a.Present,                -- 3
                        a.ExcuseReason,           -- 4
                        a.Comment,                -- 5
                        ISNULL(s.SubjectName,'') AS SubjectName,  -- 6
                        CASE WHEN a.Present = 1 THEN '✓ Присутствует'
                            WHEN a.ExcuseReason = 1 THEN '⚠️ Уважительно'
                            ELSE '✗ Прогул' END AS StatusDisplay  -- 7
                    FROM Attendance a
                    LEFT JOIN Subjects s ON a.SubjectID = s.SubjectID
                    WHERE a.UserID = @UserID
                    ORDER BY a.AttendanceDate DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    var attendances = new List<Attendance>();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            attendances.Add(new Attendance
                            {
                                AttendanceID = reader.GetInt32(0),           // ✅ ИНДЕКС 0
                                AttendanceDate = reader.GetDateTime(1),      // ✅ ИНДЕКС 1
                                LessonDate = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),  // ✅ 2
                                Present = reader.GetBoolean(3),              // ✅ 3
                                ExcuseReason = reader.GetBoolean(4),         // ✅ 4
                                Comment = reader.IsDBNull(5) ? "" : reader.GetString(5),  // ✅ 5

                                // ✅ НОВЫЕ ПОЛЯ!
                                SubjectName = reader.IsDBNull(6) ? "" : reader.GetString(6),     // ✅ 6
                                StatusDisplay = reader.GetString(7)                                // ✅ 7
                            });
                        }
                    }

                    return attendances;
                }
            }
        }
    }
}
