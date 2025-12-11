using System;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using school.Models;
using System.Data;
using System.Collections.Generic;

namespace school.Controllers
{
    /// <summary>
    /// Контроллер для работы с таблицей Users (PermissionID)
    /// </summary>
    public class UserController
    {
        private readonly string _connectionString;

        public static UserController _userController = new UserController(Form1.CONNECTION_STRING);
        public static User CurrentUser { get; set; }

        public UserController(string connectionString)
        {
            _connectionString = connectionString ?? Form1.CONNECTION_STRING;
        }

        /// <summary>
        /// Удаление пользователя (проверяет FK зависимости)
        /// </summary>
        public bool DeleteUser(User user)
        {
            if (user == null || user.UserID <= 0)
                return false;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // ✅ ПРОВЕРКА FK зависимостей
                int homeworkCount = 0, gradesCount = 0;
                using (var hwCmd = new SqlCommand("SELECT COUNT(*) FROM Homework WHERE TeacherID = @UserID", conn))
                {
                    hwCmd.Parameters.AddWithValue("@UserID", user.UserID);
                    homeworkCount = (int)hwCmd.ExecuteScalar();
                }
                using (var grCmd = new SqlCommand("SELECT COUNT(*) FROM Grades WHERE TeacherID = @UserID OR StudentID = @UserID", conn))
                {
                    grCmd.Parameters.AddWithValue("@UserID", user.UserID);
                    gradesCount = (int)grCmd.ExecuteScalar();
                }

                if (homeworkCount > 0 || gradesCount > 0)
                    throw new InvalidOperationException(
                        $"Нельзя удалить пользователя {user.FullName}. Есть ДЗ ({homeworkCount}) и оценки ({gradesCount})");

                // Удаление
                using (var cmd = new SqlCommand("DELETE FROM Users WHERE UserID = @UserID", conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public List<User> GetAllOfPredicate(string whereCondition)
        {
            var users = new List<User>();

            try
            {
                string sql = $@"
            SELECT 
                u.UserID, 
                u.FullName, 
                u.PermissionID, 
                p.PermissionName, 
                u.ClassID,
                c.ClassName  -- ✅ Добавлено название класса
            FROM Users u
            INNER JOIN Permissions p ON u.PermissionID = p.PermissionID
            LEFT JOIN Classes c ON u.ClassID = c.ClassID  -- ✅ LEFT JOIN для классов
            WHERE {whereCondition}
            ORDER BY u.FullName";

                using (var connection = new SqlConnection(Form1.CONNECTION_STRING))
                using (var command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int? classId = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4);
                            string className = reader.IsDBNull(5) ? null : reader.GetString(5);  // ✅ Индекс 5 = ClassName

                            var user = new User
                            {
                                UserID = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                PermissionID = reader.GetInt32(2),
                                PermissionName = reader.GetString(3),
                                ClassID = classId,
                                Class = className != null ? new Class
                                {
                                    ClassID = classId ?? 0,
                                    ClassName = className
                                } : null 
                            };

                            users.Add(user);
                        }
                    }
                }

                return users;
            }
            catch (Exception ex)
            {
                return new List<User>();
            }
        }

        /// <summary>
        /// Вставка нового пользователя или обновление существующего (PermissionID)
        /// </summary>
        public int InsertOrUpdateUser(User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.FullName) || user.PermissionID <= 0)
                throw new ArgumentException("Пользователь не может быть null или пустым");

            var validationContext = new ValidationContext(user);
            Validator.ValidateObject(user, validationContext, true);

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Проверка существования
                using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE UserID = @UserID", conn))
                {
                    checkCmd.Parameters.AddWithValue("@UserID", user.UserID);
                    int exists = (int)checkCmd.ExecuteScalar();

                    SqlCommand cmd;
                    if (exists == 0)
                    {
                        // ✅ НОВЫЙ пользователь
                        cmd = new SqlCommand(@"
                            INSERT INTO Users (FullName, PermissionID, ClassID) 
                            OUTPUT INSERTED.UserID 
                            VALUES (@FullName, @PermissionID, @ClassID)", conn);
                    }
                    else
                    {
                        // ✅ ОБНОВЛЕНИЕ
                        cmd = new SqlCommand(@"
                            UPDATE Users 
                            SET FullName = @FullName, PermissionID = @PermissionID, ClassID = @ClassID 
                            WHERE UserID = @UserID
                            SELECT @UserID", conn);
                    }

                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@PermissionID", user.PermissionID);
                    cmd.Parameters.AddWithValue("@ClassID", (object)user.ClassID ?? DBNull.Value);
                    if (exists > 0)
                        cmd.Parameters.AddWithValue("@UserID", user.UserID);

                    int userId = (int)cmd.ExecuteScalar();
                    user.UserID = userId;
                    return userId;
                }
            }
        }

        /// <summary>
        /// Получение пользователя по ID с PermissionName
        /// </summary>
        public User GetUserById(int userId)
        {
            if (userId <= 0) return null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
            SELECT u.UserID, u.FullName, u.PermissionID, p.PermissionName, u.ClassID 
            FROM Users u 
            LEFT JOIN Permissions p ON u.PermissionID = p.PermissionID 
            WHERE u.UserID = @UserID", conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = reader.GetInt32(0),           // ✅ UserID (индекс 0)
                                FullName = reader.GetString(1),        // ✅ FullName (индекс 1)
                                PermissionID = reader.GetInt32(2),     // ✅ PermissionID (индекс 2)
                                PermissionName = reader.IsDBNull(3) ?  // ✅ PermissionName (индекс 3)
                                    "Неизвестно" : reader.GetString(3),
                                ClassID = reader.IsDBNull(4) ?         // ✅ ClassID (индекс 4)
                                    (int?)null : reader.GetInt32(4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Возвращает StudentID по полному имени (ученики PermissionID = 1)
        /// </summary>
        public int GetStudentIdByName(string studentName)
        {
            if (string.IsNullOrWhiteSpace(studentName)) return 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "SELECT UserID FROM Users WHERE FullName = @FullName AND PermissionID = 1", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@FullName", SqlDbType.NVarChar, 100)
                    {
                        Value = studentName.Trim()
                    });

                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        /// <summary>
        /// Получение пользователя по логину и паролю
        /// </summary>
        public User GetUserByLoginPassword(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                return null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
            SELECT u.UserID, u.FullName, u.PermissionID, p.PermissionName, u.ClassID 
            FROM Users u 
            LEFT JOIN Permissions p ON u.PermissionID = p.PermissionID 
            WHERE u.FullName = @Login AND PasswordHash = @Password", conn))
                {
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User user = new User
                            {
                                UserID = reader.GetInt32(0),           // ✅ Индекс 0
                                FullName = reader.GetString(1),        // ✅ Индекс 1
                                PermissionID = reader.GetInt32(2),     // ✅ Индекс 2
                                PermissionName = reader.IsDBNull(3) ?  // ✅ Индекс 3
                                    "Неизвестно" : reader.GetString(3)
                            };

                            user.ClassID = reader.IsDBNull(4) ? null : (int?)reader.GetInt32(4);  // ✅ C# 7.3
                            return user;
                        }
                    }
                }
            }
            return null;
        }
    }
}