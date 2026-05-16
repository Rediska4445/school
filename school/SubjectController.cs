using school.Models;
using System;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Collections.Generic;
using school.Controllers;
using System.Xml.Linq;

namespace school
{
    /// <summary>
    /// Контроллер для работы с таблицей Subjects
    /// </summary>
    public class SubjectController
    {
        public static SubjectController _controller = new SubjectController(Form1.CONNECTION_STRING);

        public class SubjectChange
        {
            public string Action { get; set; } // "EDIT", "ADD", "DELETE"
            public Subject Subject { get; set; } = new Subject();
        }

        private List<SubjectChange> pendingChanges = new List<SubjectChange>();
        private readonly string _connectionString;

        public SubjectController(string connectionString)
        {
            _connectionString = connectionString ??
                Form1.CONNECTION_STRING;
        }

        /// <summary>
        /// Добавляет изменение в очередь
        /// </summary>
        public void AddSubjectChange(string action, Subject subject)
        {
            if (UserController.CurrentUser.PermissionID <= 1) return; // Только учителя/директора

            pendingChanges.Add(new SubjectChange
            {
                Action = action,
                Subject = subject
            });
        }

        /// <summary>
        /// Выполняет все изменения из очереди и очищает её
        /// </summary>
        public int CommitSubjectChanges()
        {
            if (pendingChanges.Count == 0) return 0;

            int processed = 0;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (var change in pendingChanges)
                            {
                                switch (change.Action.ToUpper())
                                {
                                    case "EDIT":
                                    case "ADD":
                                        InsertOrUpdateSubject(change.Subject);
                                        processed++;
                                        break;
                                    case "DELETE":
                                        if (DeleteSubject(change.Subject))
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
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"CommitSubjectChanges error: {ex.Message}");
            }

            return processed;
        }

        public List<Subject> GetClassSubjects(int classId)
        {
            var subjects = new List<Subject>();

            using (var conn = new SqlConnection(Form1.CONNECTION_STRING))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
            SELECT DISTINCT 
                s.SubjectID,
                s.SubjectName
            FROM Subjects s
            INNER JOIN Grades g ON s.SubjectID = g.SubjectID
            INNER JOIN Users u ON g.StudentID = u.UserID
            WHERE u.ClassID = @ClassId
            ORDER BY s.SubjectName", conn))
                {
                    cmd.Parameters.AddWithValue("@ClassId", classId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjects.Add(new Subject
                            {
                                SubjectID = reader.GetInt32(0),
                                SubjectName = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return subjects;
        }

        public List<Subject> GetAllSubjects()
        {
            var subjects = new List<Subject>();

            var connection = new SqlConnection(Form1.CONNECTION_STRING);
            var command = new SqlCommand(@"
                SELECT 
                    SubjectID,
                    SubjectName
                FROM Subjects
                ORDER BY SubjectName", connection);

            connection.Open();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var subject = new Subject
                {
                    SubjectID = reader.GetInt32(0),
                    SubjectName = reader.GetString(1)
                };
                subjects.Add(subject);
            }

            reader.Close();
            connection.Close();

            return subjects;
        }

        public bool DeleteSubject(Subject sub)
        {
            if (sub == null || sub.SubjectID <= 0)
                return false;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // ROUND 1: проверяем, есть ли ДЗ/оценки
                int homeworkCount = 0, gradesCount = 0;
                using (var checkCmd = new SqlCommand(@"
            SELECT COUNT(*) FROM Homework WHERE SubjectID = @SubjectID", conn))
                {
                    checkCmd.Parameters.AddWithValue("@SubjectID", sub.SubjectID);
                    homeworkCount = (int)checkCmd.ExecuteScalar();
                }

                using (var checkCmd = new SqlCommand(@"
            SELECT COUNT(*) FROM Grades WHERE SubjectID = @SubjectID", conn))
                {
                    checkCmd.Parameters.AddWithValue("@SubjectID", sub.SubjectID);
                    gradesCount = (int)checkCmd.ExecuteScalar();
                }

                if (homeworkCount > 0 || gradesCount > 0)
                    throw new InvalidOperationException(
                        $"Нельзя удалить предмет {sub.SubjectName}. Есть ДЗ ({homeworkCount}) и оценки ({gradesCount})");

                // ROUND 2: удаляем все связанные с предметом записи

                // Удаляем предметы в расписании
                using (var cmd = new SqlCommand(@"
            DELETE FROM Schedule
            WHERE SubjectID = @subjectId", conn))
                {
                    cmd.Parameters.AddWithValue("@subjectId", sub.SubjectID);
                    cmd.ExecuteNonQuery();
                }

                // Удаляем часы по классам
                using (var cmd = new SqlCommand(@"
            DELETE FROM SubjectClassHours
            WHERE SubjectID = @subjectId", conn))
                {
                    cmd.Parameters.AddWithValue("@subjectId", sub.SubjectID);
                    cmd.ExecuteNonQuery();
                }

                // Удаляем связи Учитель-Предмет (TeacherSubjects)
                using (var cmd = new SqlCommand(@"
            DELETE FROM TeacherSubjects
            WHERE SubjectID = @subjectId", conn))
                {
                    cmd.Parameters.AddWithValue("@subjectId", sub.SubjectID);
                    cmd.ExecuteNonQuery();
                }

                // Удаляем сам предмет
                using (var cmd = new SqlCommand("DELETE FROM Subjects WHERE SubjectID = @subjectId", conn))
                {
                    cmd.Parameters.AddWithValue("@subjectId", sub.SubjectID);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        /// <summary>
        /// [translate:Вставка нового предмета или обновление существующего]
        /// </summary>
        public int InsertOrUpdateSubject(Subject sub)
        {
            FileLogger.logger.Info($"InsertOrUpdateSubject: ID={sub.SubjectID}, Name='{sub.SubjectName}'");

            if (sub == null || string.IsNullOrWhiteSpace(sub.SubjectName))
                throw new ArgumentException("[translate:Предмет не может быть null или пустым]");

            var validationContext = new ValidationContext(sub);
            Validator.ValidateObject(sub, validationContext, true);

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                FileLogger.logger.Debug("Соединение открыто");

                using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Subjects WHERE SubjectID = @SubjectID", conn))
                {
                    checkCmd.Parameters.AddWithValue("@SubjectID", sub.SubjectID);
                    int exists = (int)checkCmd.ExecuteScalar();
                    FileLogger.logger.Info($"Предмет ID={sub.SubjectID} существует: {exists}");

                    if (exists == 0)
                    {
                        FileLogger.logger.Info("INSERT новый предмет");
                        using (var cmd = new SqlCommand(
                            "INSERT INTO Subjects (SubjectName) OUTPUT INSERTED.SubjectID VALUES (@SubjectName)", conn))
                        {
                            cmd.Parameters.AddWithValue("@SubjectName", sub.SubjectName);
                            int newId = (int)cmd.ExecuteScalar();
                            FileLogger.logger.Info($"Новый ID: {newId}");
                            return newId;
                        }
                    }
                    else
                    {
                        FileLogger.logger.Info("UPDATE существующий предмет");

                        int homeworkCount = 0, gradesCount = 0;
                        using (var hwCmd = new SqlCommand("SELECT COUNT(*) FROM Homework WHERE SubjectID = @SubjectID", conn))
                        {
                            hwCmd.Parameters.AddWithValue("@SubjectID", sub.SubjectID);
                            homeworkCount = (int)hwCmd.ExecuteScalar();
                        }
                        using (var grCmd = new SqlCommand("SELECT COUNT(*) FROM Grades WHERE SubjectID = @SubjectID", conn))
                        {
                            grCmd.Parameters.AddWithValue("@SubjectID", sub.SubjectID);
                            gradesCount = (int)grCmd.ExecuteScalar();
                        }
                        FileLogger.logger.Debug($"Homework: {homeworkCount}, Grades: {gradesCount}");

                        using (var cmd = new SqlCommand(
                            "UPDATE Subjects SET SubjectName = @SubjectName WHERE SubjectID = @SubjectID", conn))
                        {
                            cmd.Parameters.AddWithValue("@SubjectName", sub.SubjectName);
                            cmd.Parameters.AddWithValue("@SubjectID", sub.SubjectID);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            FileLogger.logger.Info($"UPDATE affected rows: {rowsAffected}");
                            return sub.SubjectID;
                        }
                    }
                }
            }
        }

        public int UpsertSubjectWithHours(
            Subject subject,
            int classId,
            int hoursPerWeek)
        {
            FileLogger.logger.Info(
                $"UpsertSubjectWithHours: " +
                $"SubjectID={subject.SubjectID}, " +
                $"Name='{subject.SubjectName}', " +
                $"ClassID={classId}, Hours={hoursPerWeek}");

            if (subject == null || string.IsNullOrWhiteSpace(subject.SubjectName))
                throw new ArgumentException("Предмет не может быть null или пустым");

            if (classId <= 0)
                throw new ArgumentException("Некорректный ClassID");

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                int resultSubjectId = 0;

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = "SELECT COUNT(*) FROM Subjects WHERE SubjectID = @SubjectID";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@SubjectID", subject.SubjectID);
                    int exists = (int)cmd.ExecuteScalar();

                    if (exists == 0)
                    {
                        FileLogger.logger.Info("Предмет не существует - вставляем новый");

                        cmd.CommandText =
                            "INSERT INTO Subjects (SubjectName) OUTPUT INSERTED.SubjectID VALUES (@SubjectName)";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@SubjectName", subject.SubjectName);
                        resultSubjectId = (int)cmd.ExecuteScalar();
                        FileLogger.logger.Info($"Вставлен новый предмет, ID={resultSubjectId}");

                        UpsertClassSubjectHoursInConnection(conn, cmd, classId, resultSubjectId, hoursPerWeek);
                    }
                    else
                    {
                        cmd.CommandText = "SELECT SubjectName FROM Subjects WHERE SubjectID = @SubjectID";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@SubjectID", subject.SubjectID);
                        var currentName = cmd.ExecuteScalar()?.ToString() ?? "";

                        cmd.CommandText = @"
                    SELECT ISNULL(Hours, 0) FROM SubjectClassHours
                    WHERE ClassID = @ClassID AND SubjectID = @SubjectID";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@ClassID", classId);
                        cmd.Parameters.AddWithValue("@SubjectID", subject.SubjectID);
                        var currentHoursObj = cmd.ExecuteScalar();
                        int currentHours = (currentHoursObj != null ? Convert.ToInt32(currentHoursObj) : 0);

                        if (!string.Equals(currentName, subject.SubjectName, StringComparison.OrdinalIgnoreCase) ||
                            currentHours != hoursPerWeek)
                        {
                            FileLogger.logger.Info(
                                "Изменение: " +
                                $"Name: '{currentName}' -> '{subject.SubjectName}', " +
                                $"Hours: {currentHours} -> {hoursPerWeek}");

                            if (!string.Equals(currentName, subject.SubjectName, StringComparison.OrdinalIgnoreCase))
                            {
                                cmd.CommandText = "UPDATE Subjects SET SubjectName = @SubjectName WHERE SubjectID = @SubjectID";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@SubjectName", subject.SubjectName);
                                cmd.Parameters.AddWithValue("@SubjectID", subject.SubjectID);
                                int rows = cmd.ExecuteNonQuery();
                                FileLogger.logger.Info($"UPDATE Subjects affected rows: {rows}");
                            }

                            UpsertClassSubjectHoursInConnection(conn, cmd, classId, subject.SubjectID, hoursPerWeek);

                            resultSubjectId = subject.SubjectID;
                        }
                        else
                        {
                            FileLogger.logger.Info("Нет изменений: имя и часы совпадают, пропускаем");
                            resultSubjectId = subject.SubjectID;
                        }
                    }
                }

                return resultSubjectId;
            }
        }

        private void UpsertClassSubjectHoursInConnection(
            SqlConnection conn,
            SqlCommand cmd,
            int classId,
            int subjectId,
            int hours)
        {
            FileLogger.logger.Info(
                $"UpsertClassSubjectHoursInConnection: " +
                $"Class={classId}, Subject={subjectId}, Hours={hours}");

            cmd.CommandTimeout = 30;
            cmd.CommandText = @"
        MERGE SubjectClassHours AS target
        USING (VALUES (@ClassID, @SubjectID, @Hours)) AS source (ClassID, SubjectID, Hours)
        ON target.ClassID = source.ClassID
           AND target.SubjectID = source.SubjectID
        WHEN MATCHED THEN
            UPDATE SET Hours = source.Hours
        WHEN NOT MATCHED THEN
            INSERT (ClassID, SubjectID, Hours)
            VALUES (source.ClassID, source.SubjectID, source.Hours);";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ClassID", classId);
            cmd.Parameters.AddWithValue("@SubjectID", subjectId);
            cmd.Parameters.AddWithValue("@Hours", hours);

            cmd.ExecuteNonQuery();
            FileLogger.logger.Info(
                $"MERGE выполнен для Class={classId}, Subject={subjectId}, Hours={hours}");
        }

        /// <summary>
        /// [translate:Получение предмета по уникальному идентификатору]
        /// </summary>
        public Subject GetSubjectById(int subjectId)
        {
            if (subjectId <= 0)
                return null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "SELECT SubjectID, SubjectName FROM Subjects WHERE SubjectID = @SubjectID", conn))
                {
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Subject
                            {
                                SubjectID = reader.GetInt32(0),
                                SubjectName = reader.GetString(1)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<TeacherSubject> GetTeacherSubjects(int teacherId)
        {
            var result = new List<TeacherSubject>();

            using (var connection = new SqlConnection(Form1.CONNECTION_STRING))
            {
                connection.Open();

                string sql = @"
            SELECT 
                ts.TeacherSubjectID,
                ts.TeacherID,
                ts.SubjectID,
                sub.SubjectName,
                u.FullName AS TeacherName
            FROM TeacherSubjects ts
            INNER JOIN Subjects sub ON ts.SubjectID = sub.SubjectID
            INNER JOIN Users u ON ts.TeacherID = u.UserID
            WHERE ts.TeacherID = @TeacherID
            ORDER BY sub.SubjectName";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        int ordTeacherSubjectID = reader.GetOrdinal("TeacherSubjectID");
                        int ordTeacherID = reader.GetOrdinal("TeacherID");
                        int ordSubjectID = reader.GetOrdinal("SubjectID");
                        int ordSubjectName = reader.GetOrdinal("SubjectName");
                        int ordTeacherName = reader.GetOrdinal("TeacherName");

                        while (reader.Read())
                        {
                            try
                            {
                                var item = new TeacherSubject
                                {
                                    TeacherSubjectID = reader.IsDBNull(ordTeacherSubjectID) ? 0 : reader.GetInt32(ordTeacherSubjectID),
                                    TeacherID = reader.IsDBNull(ordTeacherID) ? 0 : reader.GetInt32(ordTeacherID),
                                    SubjectID = reader.IsDBNull(ordSubjectID) ? 0 : reader.GetInt32(ordSubjectID),
                                    SubjectName = reader.IsDBNull(ordSubjectName) ? "" : reader.GetString(ordSubjectName),
                                    TeacherName = reader.IsDBNull(ordTeacherName) ? "" : reader.GetString(ordTeacherName)
                                };

                                result.Add(item);
                            }
                            catch (Exception exRow)
                            {
                                FileLogger.logger.Error("GetTeacherSubjects: ошибка парсинга записи", exRow);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public int GetLessonCountBySubjectName(string subjectName)
        {
            if (string.IsNullOrWhiteSpace(subjectName))
                return 0;

            var query = @"
                SELECT ISNULL(slc.LessonCount, 0) AS LessonCount
                FROM Subjects s
                LEFT JOIN SubjectLessonCount slc ON s.SubjectID = slc.SubjectID
                WHERE s.SubjectName = @SubjectName";

            using (var conn = new SqlConnection(Form1.CONNECTION_STRING))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SubjectName", subjectName.Trim());

                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public List<Subject> GetAllSubjectsForClass(int classId)
        {
            if (classId <= 0)
                return new List<Subject>();

            const string sql = @"
        SELECT DISTINCT
            s.SubjectID,
            s.SubjectName
        FROM SubjectClassHours sch
        JOIN Subjects s ON s.SubjectID = sch.SubjectID
        WHERE sch.ClassID = @ClassID
        ORDER BY s.SubjectName";

            var result = new List<Subject>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        int subjectIdOrdinal = reader.GetOrdinal("SubjectID");
                        int subjectNameOrdinal = reader.GetOrdinal("SubjectName");

                        while (reader.Read())
                        {
                            int subjectId = reader.GetInt32(subjectIdOrdinal);
                            string subjectName = reader.GetString(subjectNameOrdinal);

                            result.Add(new Subject
                            {
                                SubjectID = subjectId,
                                SubjectName = subjectName
                            });
                        }
                    }
                }
            }

            return result;
        }

        public List<Subject> GetSubjectsForClass(int classId)
        {
            const string sql = @"
            SELECT DISTINCT
                s.SubjectID,
                s.SubjectName
            FROM Subjects s
            JOIN TeacherSubjects ts ON s.SubjectID = ts.SubjectID
            WHERE
                ts.ClassID = @ClassID
                OR ts.ClassID IS NULL
            ORDER BY s.SubjectName";

            var result = new List<Subject>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        int subjectIdOrdinal = reader.GetOrdinal("SubjectID");
                        int subjectNameOrdinal = reader.GetOrdinal("SubjectName");

                        while (reader.Read())
                        {
                            int subjectId = reader.GetInt32(subjectIdOrdinal);
                            string subjectName = reader.GetString(subjectNameOrdinal);

                            result.Add(new Subject
                            {
                                SubjectID = subjectId,
                                SubjectName = subjectName
                            });
                        }
                    }
                }
            }

            return result;
        }

        public int GetHoursByClassSubject(int classId, int subjectId)
        {
            if (classId <= 0 || subjectId <= 0)
                return 0;

            const string sql = @"
        SELECT ISNULL(Hours, 0) AS LessonHours
        FROM SubjectClassHours
        WHERE ClassID = @ClassID
          AND SubjectID = @SubjectID";

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);

                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public int GetLessonCountBySubjectId(int subjectId)
        {
            if (subjectId <= 0)
                return 0;

            const string sql = @"
            SELECT ISNULL(LessonCount, 0) AS LessonCount
            FROM SubjectLessonCount
            WHERE SubjectID = @SubjectID";

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);

                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        /// <summary>
        /// Возвращает Subject по названию (или null если не найден)
        /// </summary>
        public Subject GetSubjectByName(string subjectName)
        {
            if (string.IsNullOrWhiteSpace(subjectName))
                return null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(
                    "SELECT SubjectID, SubjectName FROM Subjects WHERE SubjectName = @SubjectName", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@SubjectName", SqlDbType.NVarChar, 50)
                    {
                        Value = subjectName.Trim()
                    });

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int idOrdinal = reader.GetOrdinal("SubjectID");
                            int nameOrdinal = reader.GetOrdinal("SubjectName");

                            return new Subject
                            {
                                SubjectID = reader.IsDBNull(idOrdinal) ? 0 : reader.GetInt32(idOrdinal),
                                SubjectName = reader.IsDBNull(nameOrdinal) ? "" : reader.GetString(nameOrdinal)
                            };
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Возвращает SubjectID по названию предмета
        /// </summary>
        public int GetSubjectIdByName(string subjectName)
        {
            if (string.IsNullOrWhiteSpace(subjectName)) return 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "SELECT SubjectID FROM Subjects WHERE SubjectName = @SubjectName", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@SubjectName", SqlDbType.NVarChar, 50)
                    {
                        Value = subjectName.Trim()
                    });

                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }
    }
}
