using System;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using school.Models;

namespace school.Controllers
{
    /// <summary>
    /// [translate:Контроллер для работы с таблицей Users]
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
        /// [translate:Удаление пользователя (проверяет FK зависимости)]
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
                        $"[translate:Нельзя удалить пользователя {user.FullName}. Есть ДЗ ({homeworkCount}) и оценки ({gradesCount})]");

                // Удаление
                using (var cmd = new SqlCommand("DELETE FROM Users WHERE UserID = @UserID", conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        /// <summary>
        /// [translate:Вставка нового пользователя или обновление существующего]
        /// </summary>
        public int InsertOrUpdateUser(User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.FullName) || string.IsNullOrWhiteSpace(user.Role))
                throw new ArgumentException("[translate:Пользователь не может быть null или пустым]");

            // Валидация Role
            if (user.Role != "Ученик" && user.Role != "Учитель")
                throw new ArgumentException("[translate:Role должен быть 'Ученик' или 'Учитель']");

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
                        // НОВЫЙ пользователь
                        cmd = new SqlCommand(@"
                            INSERT INTO Users (FullName, Role, ClassID) 
                            OUTPUT INSERTED.UserID 
                            VALUES (@FullName, @Role, @ClassID)", conn);
                        cmd.Parameters.AddWithValue("@FullName", user.FullName);
                        cmd.Parameters.AddWithValue("@Role", user.Role);
                        cmd.Parameters.AddWithValue("@ClassID", (object)user.ClassID ?? DBNull.Value);
                        int newId = (int)cmd.ExecuteScalar();
                        user.UserID = newId;
                        return newId;
                    }
                    else
                    {
                        // ОБНОВЛЕНИЕ
                        cmd = new SqlCommand(@"
                            UPDATE Users 
                            SET FullName = @FullName, Role = @Role, ClassID = @ClassID 
                            WHERE UserID = @UserID", conn);
                        cmd.Parameters.AddWithValue("@FullName", user.FullName);
                        cmd.Parameters.AddWithValue("@Role", user.Role);
                        cmd.Parameters.AddWithValue("@ClassID", (object)user.ClassID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@UserID", user.UserID);
                        cmd.ExecuteNonQuery();
                        return user.UserID;
                    }
                }
            }
        }

        public User GetUserById(int userId)
        {
            if (userId <= 0) return null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "SELECT UserID, FullName, Role, ClassID FROM Users WHERE UserID = @UserID", conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Role = reader.GetString(2),
                                ClassID = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3)  // ✅ C# 7.3
                            };
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// [translate:Получение пользователя по логину и паролю (проверка аутентификации)]
        /// </summary>
        public User GetUserByLoginPassword(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                return null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
            SELECT UserID, FullName, Role, ClassID 
            FROM Users 
            WHERE FullName = @Login AND PasswordHash = @Password", conn))
                {
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@Password", password);  // Добавляем проверку пароля

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User user = new User
                            {
                                UserID = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Role = reader.GetString(2)
                            };

                            if (reader.IsDBNull(3))
                                user.ClassID = null;
                            else
                                user.ClassID = reader.GetInt32(3);

                            return user;
                        }
                    }
                }
            }
            return null;
        }
    }
}
