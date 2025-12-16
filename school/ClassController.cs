using Microsoft.Data.SqlClient;
using school.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace school.Controllers
{
    /// <summary>
    ///     Контроллер для работы с таблицей Classes
    /// </summary>
    public class ClassController
    {
        public class ClassChange
        {
            public string Action { get; set; } // "EDIT", "ADD", "DELETE"
            public Class Class { get; set; } = new Class();
        }

        private List<ClassChange> pendingChanges = new List<ClassChange>();

        public static ClassController _controller = new ClassController(Form1.CONNECTION_STRING);

        private readonly string _connectionString;

        public ClassController(string connectionString)
        {
            _connectionString = connectionString ?? Form1.CONNECTION_STRING;
        }

        /// <summary>
        /// Добавляет изменение класса в очередь
        /// </summary>
        public void AddClassChange(string action, Class classItem)
        {
            pendingChanges.Add(new ClassChange
            {
                Action = action,
                Class = classItem
            });
        }

        /// <summary>
        /// Выполняет все изменения из очереди и очищает её
        /// </summary>
        public int CommitClassChanges()
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
                                        InsertOrUpdateClass(change.Class);
                                        processed++;
                                        break;
                                    case "DELETE":
                                        if (DeleteClass(change.Class))
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
                FileLogger.logger.Error($"CommitClassChanges error: {ex.Message}");
            }

            return processed;
        }

        /// <summary>
        ///     Удаление класса по объекту Class (по ClassID)
        /// </summary>
        public bool DeleteClass(Class cls)
        {
            if (cls == null || cls.ClassID <= 0)
                return false;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("DELETE FROM Classes WHERE ClassID = @ClassID", conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", cls.ClassID);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        /// <summary>
        ///     Вставка нового класса или обновление существующего
        /// </summary>
        /// <summary>
        /// Вставка нового класса или обновление существующего
        /// Если ClassID < 0, то INSERT с автоинкрементом
        /// </summary>
        public int InsertOrUpdateClass(Class cls)
        {
            if (cls == null || string.IsNullOrWhiteSpace(cls.ClassName))
                throw new ArgumentException("Класс не может быть null или пустым");

            var validationContext = new ValidationContext(cls);
            Validator.ValidateObject(cls, validationContext, true);

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                if (cls.ClassID < 0)
                {
                    using (var cmd = new SqlCommand(
                        "INSERT INTO Classes (ClassName) OUTPUT INSERTED.ClassID VALUES (@ClassName)", conn))
                    {
                        cmd.Parameters.AddWithValue("@ClassName", cls.ClassName);
                        int newId = (int)cmd.ExecuteScalar();
                        cls.ClassID = newId; 
                        return newId;
                    }
                }
                else 
                {
                    using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Classes WHERE ClassID = @ClassID", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@ClassID", cls.ClassID);
                        int exists = (int)checkCmd.ExecuteScalar();

                        if (exists == 0)
                            throw new InvalidOperationException($"Класс с ID {cls.ClassID} не найден");

                        using (var cmd = new SqlCommand(
                        "UPDATE Classes SET ClassName = @ClassName WHERE ClassID = @ClassID", conn))
                        {
                            cmd.Parameters.AddWithValue("@ClassName", cls.ClassName);
                            cmd.Parameters.AddWithValue("@ClassID", cls.ClassID);
                            cmd.ExecuteNonQuery();
                            return cls.ClassID;
                        }
                    }
                }
            }
        }

        public Class GetClassById(int classId)
        {
            if (classId <= 0)
                return null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "SELECT ClassID, ClassName FROM Classes WHERE ClassID = @ClassID", conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Class
                            {
                                ClassID = reader.GetInt32(reader.GetOrdinal("ClassID")),
                                ClassName = reader.GetString(reader.GetOrdinal("ClassName"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public Class GetClassByName(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
                return null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "SELECT ClassID, ClassName FROM Classes WHERE ClassName = @ClassName", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@ClassName", SqlDbType.NVarChar, 10)
                    {
                        Value = className.Trim()
                    });

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Class
                            {
                                ClassID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                ClassName = reader.IsDBNull(1) ? "" : reader.GetString(1)
                            };
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///     Возвращает список всех классов из БД
        /// </summary>
        public List<Class> GetAllClasses()
        {
            var classes = new List<Class>();

            SqlConnection connection = null;
            SqlDataReader reader = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                SqlCommand cmd = new SqlCommand("SELECT ClassID, ClassName FROM Classes ORDER BY ClassID", connection);

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    classes.Add(new Class
                    {
                        ClassID = reader.GetInt32(reader.GetOrdinal("ClassID")),
                        ClassName = reader.GetString(reader.GetOrdinal("ClassName"))
                    });
                }
            }
            finally
            {
                reader?.Dispose();
                connection?.Dispose();
            }

            return classes;
        }
    }
}
