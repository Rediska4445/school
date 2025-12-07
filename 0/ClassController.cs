using Microsoft.Data.SqlClient;
using RedSqlConnector;
using school.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school.Controllers
{
    /// <summary>
    /// [translate:Контроллер для работы с таблицей Classes]
    /// </summary>
    public class ClassController
    {
        private readonly string _connectionString;

        public ClassController(string connectionString)
        {
            _connectionString = connectionString ?? Form1.CONNECTION_STRING;
        }

        /// <summary>
        /// [translate:Удаление класса по объекту Class (по ClassID)]
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
        /// [translate:Вставка нового класса или обновление существующего]
        /// </summary>
        public int InsertOrUpdateClass(Class cls)
        {
            if (cls == null || string.IsNullOrWhiteSpace(cls.ClassName))
                throw new ArgumentException("[translate:Класс не может быть null или пустым]");

            // Валидация
            var validationContext = new ValidationContext(cls);
            Validator.ValidateObject(cls, validationContext, true);

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Проверяем существование
                using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Classes WHERE ClassID = @ClassID", conn))
                {
                    checkCmd.Parameters.AddWithValue("@ClassID", cls.ClassID);
                    int exists = (int)checkCmd.ExecuteScalar();

                    SqlCommand cmd;
                    if (exists == 0)
                    {
                        // НОВЫЙ класс
                        cmd = new SqlCommand(
                            "INSERT INTO Classes (ClassName) OUTPUT INSERTED.ClassID VALUES (@ClassName)", conn);
                        cmd.Parameters.AddWithValue("@ClassName", cls.ClassName);
                        int newId = (int)cmd.ExecuteScalar();
                        cls.ClassID = newId;
                        return newId;
                    }
                    else
                    {
                        // ОБНОВЛЕНИЕ
                        cmd = new SqlCommand(
                            "UPDATE Classes SET ClassName = @ClassName WHERE ClassID = @ClassID", conn);
                        cmd.Parameters.AddWithValue("@ClassName", cls.ClassName);
                        cmd.Parameters.AddWithValue("@ClassID", cls.ClassID);
                        cmd.ExecuteNonQuery();
                        return cls.ClassID;
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

    }
}
