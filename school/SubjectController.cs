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

        /// <summary>
        /// [translate:Удаление предмета (проверяет FK зависимости)]
        /// </summary>
        public bool DeleteSubject(Subject sub)
        {
            if (sub == null || sub.SubjectID <= 0)
                return false;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // ✅ ПРОВЕРКА FK ДО удаления
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
                        $"[translate:Нельзя удалить предмет {sub.SubjectName}. Есть ДЗ ({homeworkCount}) и оценки ({gradesCount})]");

                // ✅ Теперь безопасно удаляем
                using (var cmd = new SqlCommand("DELETE FROM Subjects WHERE SubjectID = @SubjectID", conn))
                {
                    cmd.Parameters.AddWithValue("@SubjectID", sub.SubjectID);
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

                // ✅ Проверяем существование ID
                using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Subjects WHERE SubjectID = @SubjectID", conn))
                {
                    checkCmd.Parameters.AddWithValue("@SubjectID", sub.SubjectID);
                    int exists = (int)checkCmd.ExecuteScalar();
                    FileLogger.logger.Info($"Предмет ID={sub.SubjectID} существует: {exists}");

                    if (exists == 0)
                    {
                        // ✅ НЕТ - ДОБАВЛЯЕМ (автоинкремент)
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
                        // ✅ ЕСТЬ - ИЗМЕНЯЕМ
                        FileLogger.logger.Info("UPDATE существующий предмет");

                        // ПРОВЕРКА FK
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

                        // UPDATE
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
