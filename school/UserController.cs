using System;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using school.Models;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;

namespace school.Controllers
{
    /// <summary>
    /// Контроллер для работы с таблицей Users (PermissionID)
    /// </summary>
    public class UserController
    {
        /// <summary>
        /// Модель изменения пользователя
        /// </summary>
        public class UserChange
        {
            public string Action { get; set; } // "EDIT", "ADD", "DELETE"
            public User User { get; set; } = new User();
        }

        private readonly string _connectionString;

        public static UserController _userController = new UserController(Form1.CONNECTION_STRING);
        public static User CurrentUser { get; set; }

        private List<UserChange> pendingChanges = new List<UserChange>();

        public UserController(string connectionString)
        {
            _connectionString = connectionString ?? Form1.CONNECTION_STRING;
        }

        /// <summary>
        /// Добавляет изменение пользователя в очередь
        /// </summary>
        public void AddUserChange(string action, User userModel)
        {
            pendingChanges.Add(new UserChange
            {
                Action = action.ToUpper(),
                User = userModel
            });

            FileLogger.logger.Info($"Добавлено изменение пользователя: {action} (ID: {userModel.UserID}, {userModel.FullName})");
        }

        /// <summary>
        /// Выполняет все изменения пользователей и очищает очередь
        /// </summary>
        public int CommitUserChanges()
        {
            if (pendingChanges.Count == 0) return 0;

            int processed = 0;
            try
            {
                using (var connection = new SqlConnection(Form1.CONNECTION_STRING))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (var change in pendingChanges)
                            {
                                switch (change.Action)
                                {
                                    case "EDIT":
                                    case "ADD":
                                        UpsertUser(change.User);
                                        processed++;
                                        break;
                                    case "DELETE":
                                        // DeleteUserInTransaction(change.User.UserID, connection, transaction);
                                        // TODO: Add "isActive" to User model
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
                FileLogger.logger.Info($"Сохранено {processed} изменений пользователей");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"Ошибка сохранения пользователей: {ex.Message}");
                throw;
            }
            return processed;
        }

        /// <summary>
        /// Вставляет или обновляет пользователя по логике UPSERT
        /// 1) Если ID >= 0 и существует → UPDATE
        /// 2) Если ID >= 0 но не существует → INSERT (новый ID)
        /// 3) Если ID < 0 → INSERT с автоинкрементом IDENTITY(1,1)
        /// </summary>
        public int UpsertUser(User userModel)
        {
            using (var connection = new SqlConnection(Form1.CONNECTION_STRING))
            {
                connection.Open();

                bool exists = false;
                if (userModel.UserID >= 0)
                {
                    var existsQuery = "SELECT COUNT(*) FROM Users WHERE UserID = @UserID";
                    using (var checkCmd = new SqlCommand(existsQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@UserID", userModel.UserID);
                        exists = (int)checkCmd.ExecuteScalar() > 0;
                    }
                }

                if (exists)
                {
                    var updateQuery = @"
                        UPDATE Users 
                        SET FullName = @FullName, 
                            PasswordHash = @PasswordHash,
                            PermissionID = @PermissionID, 
                            ClassID = @ClassID
                        WHERE UserID = @UserID";

                    using (var updateCmd = new SqlCommand(updateQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@FullName", userModel.FullName);
                        updateCmd.Parameters.AddWithValue("@PasswordHash", userModel.Password ?? "default_hash_" + Guid.NewGuid().ToString("N").Substring(0, 32));
                        updateCmd.Parameters.AddWithValue("@PermissionID", userModel.PermissionID);
                        updateCmd.Parameters.AddWithValue("@ClassID", userModel.ClassID ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@UserID", userModel.UserID);

                        updateCmd.ExecuteNonQuery();
                        FileLogger.logger.Info($"Обновлен пользователь ID={userModel.UserID}: {userModel.FullName}");
                        return userModel.UserID;
                    }
                }
                else
                {
                    var insertQuery = @"
                        INSERT INTO Users (FullName, PasswordHash, PermissionID, ClassID)
                        OUTPUT INSERTED.UserID
                        VALUES (@FullName, @PasswordHash, @PermissionID, @ClassID)";

                    using (var insertCmd = new SqlCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@FullName", userModel.FullName);
                        insertCmd.Parameters.AddWithValue("@PasswordHash", userModel.Password ?? "default_hash_" + Guid.NewGuid().ToString("N").Substring(0, 32));
                        insertCmd.Parameters.AddWithValue("@PermissionID", userModel.PermissionID);
                        insertCmd.Parameters.AddWithValue("@ClassID", userModel.ClassID ?? (object)DBNull.Value);

                        int newId = (int)insertCmd.ExecuteScalar();
                        userModel.UserID = newId;
                        FileLogger.logger.Info($"Создан новый пользователь ID={newId}: {userModel.FullName}");
                        return newId;
                    }
                }
            }
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
                u.PasswordHash,  -- ✅ ДОБАВЛЕНО!
                u.PermissionID, 
                p.PermissionName, 
                u.ClassID,
                c.ClassName
            FROM Users u
            INNER JOIN Permissions p ON u.PermissionID = p.PermissionID
            LEFT JOIN Classes c ON u.ClassID = c.ClassID
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
                            int? classId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5);  // ✅ Сдвинут индекс
                            string className = reader.IsDBNull(6) ? null : reader.GetString(6);   // ✅ Сдвинут индекс

                            var user = new User
                            {
                                UserID = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Password = reader.IsDBNull(2) ? null : reader.GetString(2),  // ✅ ДОБАВЛЕНО!
                                PermissionID = reader.GetInt32(3),
                                PermissionName = reader.GetString(4),
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
                FileLogger.logger.Error($"GetAllOfPredicate ошибка: {ex.Message}");
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
                
                using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE UserID = @UserID", conn))
                {
                    checkCmd.Parameters.AddWithValue("@UserID", user.UserID);
                    int exists = (int)checkCmd.ExecuteScalar();

                    SqlCommand cmd;
                    if (exists == 0)
                    {
                        cmd = new SqlCommand(@"
                            INSERT INTO Users (FullName, PermissionID, ClassID) 
                            OUTPUT INSERTED.UserID 
                            VALUES (@FullName, @PermissionID, @ClassID)", conn);
                    }
                    else
                    {
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
        /// Регистрирует нового пользователя через хранимую процедуру
        /// </summary>
        /// <param name="fullName">ФИО пользователя</param>
        /// <param name="password">Пароль</param>
        /// <param name="permissionID">1=Ученик, 2=Учитель, 3=Директор</param>
        /// <param name="classID">ID класса (NULL для учителя/директора)</param>
        /// <returns>Новый UserID или 0 при ошибке</returns>
        public int RegisterUser(string fullName, string password, int permissionID, int? classID = null)
        {
            int newUserID = 0;

            using (SqlConnection conn = new SqlConnection(Form1.CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand("sp_RegisterUserSimple", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FullName", fullName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Password", password ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PermissionID", permissionID);
                    cmd.Parameters.AddWithValue("@ClassID", classID ?? (object)DBNull.Value);

                    SqlParameter outParam = new SqlParameter("@NewUserID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outParam);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        newUserID = (int)outParam.Value;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception($"Ошибка базы данных: {ex.Message}", ex);
                    }
                }
            }
            return newUserID;
        }

        /// <summary>
        /// Регистрирует пользователя с сообщением для UI
        /// </summary>
        public bool RegisterUserWithMessage(string fullName, string password, int permissionID, int? classID = null)
        {
            try
            {
                int userID = RegisterUser(fullName, password, permissionID, classID);
                if (userID > 0)
                {
                    MessageBox.Show($"Пользователь '{fullName}' зарегистрирован! ID: {userID}",
                                   "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
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
            SELECT u.UserID, u.FullName, u.PermissionID, p.PermissionName, u.ClassID,
                   c.ClassID AS Class_ClassID, c.ClassName  -- ✅ ДОБАВЛЯЕМ КЛАСС
            FROM Users u 
            LEFT JOIN Permissions p ON u.PermissionID = p.PermissionID
            LEFT JOIN Classes c ON u.ClassID = c.ClassID  -- ✅ JOIN с классами
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
                                UserID = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                PermissionID = reader.GetInt32(2),
                                PermissionName = reader.IsDBNull(3) ? "Неизвестно" : reader.GetString(3)
                            };

                            user.ClassID = reader.IsDBNull(4) ? null : (int?)reader.GetInt32(4);

                            if (!reader.IsDBNull(5) && !reader.IsDBNull(6))
                            {
                                user.Class = new Class
                                {
                                    ClassID = reader.GetInt32(5),  // Class_ClassID
                                    ClassName = reader.GetString(6)
                                };
                            }

                            return user;
                        }
                    }
                }
            }
            return null;
        }

        public int GetPermission(string v)
        {
            if(v.Contains("Директор"))
            {
                return 3;
            }
            else if (v.Contains("Учитель"))
            {
                return 2;
            } 
            else
            {
                return 1;
            }
        }
    }
}