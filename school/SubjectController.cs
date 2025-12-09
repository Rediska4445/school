using school.Models;
using System;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace school
{
    /// <summary>
    /// [translate:Контроллер для работы с таблицей Subjects]
    /// </summary>
    public class SubjectController
    {
        public static SubjectController _controller = new SubjectController(Form1.CONNECTION_STRING);   

        private readonly string _connectionString;

        public SubjectController(string connectionString)
        {
            _connectionString = connectionString ??
                Form1.CONNECTION_STRING;
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
            if (sub == null || string.IsNullOrWhiteSpace(sub.SubjectName))
                throw new ArgumentException("[translate:Предмет не может быть null или пустым]");

            var validationContext = new ValidationContext(sub);
            Validator.ValidateObject(sub, validationContext, true);

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // ✅ ПРОВЕРКА FK для ЛЮБОГО обновления (даже если ID = 0)
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

                if (homeworkCount > 0 || gradesCount > 0)
                    throw new InvalidOperationException(
                        $"[translate:Нельзя изменить предмет. Используется в ДЗ ({homeworkCount}) и оценках ({gradesCount})]");

                // Проверяем существование
                using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Subjects WHERE SubjectID = @SubjectID", conn))
                {
                    checkCmd.Parameters.AddWithValue("@SubjectID", sub.SubjectID);
                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists == 0)
                    {
                        // НОВЫЙ предмет
                        using (var cmd = new SqlCommand(
                            "INSERT INTO Subjects (SubjectName) OUTPUT INSERTED.SubjectID VALUES (@SubjectName)", conn))
                        {
                            cmd.Parameters.AddWithValue("@SubjectName", sub.SubjectName);
                            int newId = (int)cmd.ExecuteScalar();
                            sub.SubjectID = newId;
                            return newId;
                        }
                    }
                    else
                    {
                        // ОБНОВЛЕНИЕ (проверка FK уже сделана выше)
                        using (var cmd = new SqlCommand(
                            "UPDATE Subjects SET SubjectName = @SubjectName WHERE SubjectID = @SubjectID", conn))
                        {
                            cmd.Parameters.AddWithValue("@SubjectName", sub.SubjectName);
                            cmd.Parameters.AddWithValue("@SubjectID", sub.SubjectID);
                            cmd.ExecuteNonQuery();
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
