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
    public class TeacherController
    {
        private readonly string _connectionString;

        public static TeacherController _controller = new TeacherController(Form1.CONNECTION_STRING);

        public TeacherController(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<User> GetStudentsByClass(Class classObj)
        {
            FileLogger.logger.Info("=== GetStudentsByClass НАЧАЛО ===");

            // ✅ Проверки (без изменений)
            FileLogger.logger.Info($"ClassID: {classObj?.ClassID ?? 0}");

            if (classObj == null)
            {
                FileLogger.logger.Error("classObj = NULL!");
                return new List<User>();
            }

            if (classObj?.ClassID == 0)
            {
                FileLogger.logger.Error("ClassID = 0 или null!");
                return new List<User>();
            }

            if (string.IsNullOrEmpty(_connectionString))
            {
                FileLogger.logger.Error("ConnectionString пустой!");
                return new List<User>();
            }

            var students = new List<User>();

            try
            {
                FileLogger.logger.Info("Открываем соединение...");
                var connection = new SqlConnection(_connectionString);
                var command = new SqlCommand(@"
            SELECT 
                u.UserID, u.FullName, u.PermissionID, u.ClassID, p.PermissionName,
                c.ClassID AS Class_ClassID, c.ClassName  -- ✅ ПОЛНЫЙ КЛАСС
            FROM Users u
            INNER JOIN Permissions p ON u.PermissionID = p.PermissionID
            INNER JOIN Classes c ON u.ClassID = c.ClassID
            WHERE u.ClassID = @ClassID
              AND p.PermissionName = N'Ученик'
            ORDER BY u.FullName", connection);

                command.Parameters.AddWithValue("@ClassID", classObj.ClassID);

                connection.Open();
                FileLogger.logger.Info("Соединение открыто. Читаем данные...");

                var reader = command.ExecuteReader();
                int rowCount = 0;

                while (reader.Read())
                {
                    rowCount++;
                    FileLogger.logger.Debug($"Строка {rowCount}: {reader.GetString(1)} (ID:{reader.GetInt32(0)})");

                    var student = new User
                    {
                        UserID = reader.GetInt32(0),
                        FullName = reader.GetString(1),
                        PermissionID = reader.GetInt32(2),
                        ClassID = reader.GetInt32(3),
                        PermissionName = reader.GetString(4),

                        // ✅ ПОЛНЫЙ CLASS объект
                        Class = new Class
                        {
                            ClassID = reader.GetInt32(5),     // Class_ClassID
                            ClassName = reader.GetString(6)   // ClassName
                        }
                    };
                    students.Add(student);
                }

                FileLogger.logger.Info($"Найдено учеников: {students.Count}");
                reader.Close();
                connection.Close();
                FileLogger.logger.Info("=== GetStudentsByClass КОНЕЦ ===");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"ИСКЛЮЧЕНИЕ: {ex.Message}");
                FileLogger.logger.Error($"StackTrace: {ex.StackTrace}");
            }

            return students;
        }

        /// <summary>
        /// Получает ВСЕ домашние задания, заданные учителем за период
        /// </summary>
        public List<Homework> GetTeacherHomework(DateTime startDate, DateTime endDate, User teacher)
        {
            var homeworkList = new List<Homework>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                    SELECT 
                        h.HomeworkID,
                        h.AssignmentDate,
                        h.ClassID,
                        c.ClassName,
                        h.SubjectID,
                        s.SubjectName,
                        h.Description,
                        h.TeacherID,
                        u.FullName AS TeacherFullName
                    FROM Homework h
                    JOIN Classes c ON h.ClassID = c.ClassID
                    JOIN Subjects s ON h.SubjectID = s.SubjectID
                    JOIN Users u ON h.TeacherID = u.UserID
                    WHERE h.TeacherID = @TeacherID
                        AND h.AssignmentDate BETWEEN @StartDate AND @EndDate
                    ORDER BY h.AssignmentDate DESC, c.ClassName", conn))
                {
                    cmd.Parameters.AddWithValue("@TeacherID", teacher.UserID);
                    cmd.Parameters.AddWithValue("@StartDate", startDate.Date);
                    cmd.Parameters.AddWithValue("@EndDate", endDate.Date);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            homeworkList.Add(new Homework
                            {
                                HomeworkID = reader.GetInt32(reader.GetOrdinal("HomeworkID")),
                                AssignmentDate = reader.GetDateTime(reader.GetOrdinal("AssignmentDate")),
                                ClassID = reader.GetInt32(reader.GetOrdinal("ClassID")),
                                Class = new Class { ClassName = reader.GetString(reader.GetOrdinal("ClassName")) },
                                SubjectID = reader.GetInt32(reader.GetOrdinal("SubjectID")),
                                Subject = new Subject { SubjectName = reader.GetString(reader.GetOrdinal("SubjectName")) },
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                TeacherID = reader.GetInt32(reader.GetOrdinal("TeacherID")),
                                Teacher = new User { FullName = reader.GetString(reader.GetOrdinal("TeacherFullName")) }
                            });
                        }
                    }
                }
            }
            return homeworkList;
        }

        /// <summary>
        /// Получает ВСЕ оценки, выставленные учителем за период
        /// </summary>
        public List<Grade> GetTeacherGrades(DateTime startDate, DateTime endDate, User teacher)
        {
            var gradesList = new List<Grade>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                    SELECT 
                        g.GradeID,
                        g.GradeDate,
                        g.StudentID,
                        stu.FullName AS StudentFullName,
                        stu.ClassID,
                        c.ClassName,
                        g.SubjectID,
                        s.SubjectName,
                        g.GradeValue,
                        g.TeacherID,
                        t.FullName AS TeacherFullName
                    FROM Grades g
                    JOIN Users stu ON g.StudentID = stu.UserID
                    LEFT JOIN Classes c ON stu.ClassID = c.ClassID
                    JOIN Subjects s ON g.SubjectID = s.SubjectID
                    JOIN Users t ON g.TeacherID = t.UserID
                    WHERE g.TeacherID = @TeacherID
                        AND g.GradeDate BETWEEN @StartDate AND @EndDate
                    ORDER BY g.GradeDate DESC, stu.FullName", conn))
                {
                    cmd.Parameters.AddWithValue("@TeacherID", teacher.UserID);
                    cmd.Parameters.AddWithValue("@StartDate", startDate.Date);
                    cmd.Parameters.AddWithValue("@EndDate", endDate.Date);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            gradesList.Add(new Grade
                            {
                                GradeID = reader.GetInt32(reader.GetOrdinal("GradeID")),
                                GradeDate = reader.GetDateTime(reader.GetOrdinal("GradeDate")),
                                StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                                Student = new User
                                {
                                    FullName = reader.GetString(reader.GetOrdinal("StudentFullName")),
                                    ClassID = reader.IsDBNull(reader.GetOrdinal("ClassID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ClassID")),
                                    Class = new Class
                                    {
                                        ClassID = reader.IsDBNull(reader.GetOrdinal("ClassID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ClassID")),
                                        ClassName = reader.IsDBNull(reader.GetOrdinal("ClassName")) ? "" : reader.GetString(reader.GetOrdinal("ClassName"))
                                    }
                                },
                                SubjectID = reader.GetInt32(reader.GetOrdinal("SubjectID")),
                                Subject = new Subject
                                {
                                    SubjectName = reader.GetString(reader.GetOrdinal("SubjectName"))
                                },
                                GradeValue = reader.GetByte(reader.GetOrdinal("GradeValue")),
                                TeacherID = reader.GetInt32(reader.GetOrdinal("TeacherID")),
                                Teacher = new User
                                {
                                    FullName = reader.GetString(reader.GetOrdinal("TeacherFullName"))
                                }
                            });
                        }
                    }
                }
            }
            return gradesList;
        }

        /// <summary>
        /// Получает ВСЕ оценки учеников класса за период
        /// </summary>
        public List<Grade> GetClassGrades(DateTime startDate, DateTime endDate, int classId)
        {
            var gradesList = new List<Grade>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
            SELECT 
                g.GradeID,
                g.GradeDate,
                g.StudentID,
                stu.FullName AS StudentFullName,
                stu.ClassID,
                c.ClassName,
                g.SubjectID,
                s.SubjectName,
                g.GradeValue,
                g.TeacherID,
                t.FullName AS TeacherFullName
            FROM Grades g
            JOIN Users stu ON g.StudentID = stu.UserID
            LEFT JOIN Classes c ON stu.ClassID = c.ClassID
            JOIN Subjects s ON g.SubjectID = s.SubjectID
            JOIN Users t ON g.TeacherID = t.UserID
            WHERE stu.ClassID = @ClassID
                AND g.GradeDate BETWEEN @StartDate AND @EndDate
            ORDER BY stu.FullName, g.GradeDate DESC, s.SubjectName", conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    cmd.Parameters.AddWithValue("@StartDate", startDate.Date);
                    cmd.Parameters.AddWithValue("@EndDate", endDate.Date);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            gradesList.Add(new Grade
                            {
                                GradeID = reader.GetInt32(reader.GetOrdinal("GradeID")),
                                GradeDate = reader.GetDateTime(reader.GetOrdinal("GradeDate")),
                                StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                                Student = new User
                                {
                                    FullName = reader.GetString(reader.GetOrdinal("StudentFullName")),
                                    ClassID = reader.IsDBNull(reader.GetOrdinal("ClassID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ClassID")),
                                    Class = new Class
                                    {
                                        ClassID = reader.IsDBNull(reader.GetOrdinal("ClassID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ClassID")),
                                        ClassName = reader.IsDBNull(reader.GetOrdinal("ClassName")) ? "" : reader.GetString(reader.GetOrdinal("ClassName"))
                                    }
                                },
                                SubjectID = reader.GetInt32(reader.GetOrdinal("SubjectID")),
                                Subject = new Subject
                                {
                                    SubjectName = reader.GetString(reader.GetOrdinal("SubjectName"))
                                },
                                GradeValue = reader.GetByte(reader.GetOrdinal("GradeValue")),
                                TeacherID = reader.GetInt32(reader.GetOrdinal("TeacherID")),
                                Teacher = new User
                                {
                                    FullName = reader.GetString(reader.GetOrdinal("TeacherFullName"))
                                }
                            });
                        }
                    }
                }
            }
            return gradesList;
        }

        /// <summary>
        /// Вставляет или обновляет домашнее задание (UPSERT)
        /// Проверяет TeacherSubjects на право учителя вести предмет/класс
        /// </summary>
        /// <returns>ID вставленной/обновленной записи</returns>
        public int UpsertHomework(Homework homework)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // ✅ 1. ПРОВЕРКА ПРАВ учителя на предмет/класс
                using (var checkCmd = new SqlCommand(@"
            SELECT COUNT(*) FROM TeacherSubjects ts
            WHERE ts.TeacherID = @TeacherID 
                AND ts.SubjectID = @SubjectID
                AND (@ClassID IS NULL OR ts.ClassID = @ClassID OR ts.ClassID IS NULL)", conn))
                {
                    checkCmd.Parameters.AddWithValue("@TeacherID", homework.TeacherID);
                    checkCmd.Parameters.AddWithValue("@SubjectID", homework.SubjectID);
                    checkCmd.Parameters.AddWithValue("@ClassID", homework.ClassID);

                    int hasPermission = (int)checkCmd.ExecuteScalar();
                    if (hasPermission == 0)
                        throw new InvalidOperationException("Учитель не ведет этот предмет в указанном классе");
                }

                // ✅ 2. UPSERT: если существует - UPDATE, иначе INSERT
                using (var upsertCmd = new SqlCommand(@"
            IF EXISTS (
                SELECT 1 FROM Homework 
                WHERE AssignmentDate = @AssignmentDate 
                    AND ClassID = @ClassID 
                    AND SubjectID = @SubjectID 
                    AND TeacherID = @TeacherID
            )
            BEGIN
                -- UPDATE существующего
                UPDATE Homework 
                SET Description = @Description
                WHERE AssignmentDate = @AssignmentDate 
                    AND ClassID = @ClassID 
                    AND SubjectID = @SubjectID 
                    AND TeacherID = @TeacherID;
                
                SELECT HomeworkID FROM Homework 
                WHERE AssignmentDate = @AssignmentDate 
                    AND ClassID = @ClassID 
                    AND SubjectID = @SubjectID 
                    AND TeacherID = @TeacherID;
            END
            ELSE
            BEGIN
                -- INSERT нового
                INSERT INTO Homework (AssignmentDate, ClassID, SubjectID, Description, TeacherID)
                OUTPUT INSERTED.HomeworkID
                VALUES (@AssignmentDate, @ClassID, @SubjectID, @Description, @TeacherID);
            END", conn))
                {
                    upsertCmd.Parameters.AddWithValue("@AssignmentDate", homework.AssignmentDate.Date);
                    upsertCmd.Parameters.AddWithValue("@ClassID", homework.ClassID);
                    upsertCmd.Parameters.AddWithValue("@SubjectID", homework.SubjectID);
                    upsertCmd.Parameters.AddWithValue("@Description", homework.Description ?? "");
                    upsertCmd.Parameters.AddWithValue("@TeacherID", homework.TeacherID);

                    return (int)upsertCmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Возвращает User по полному имени с указанными ролями (PermissionID)
        /// </summary>
        /// <param name="teacherName">Полное имя пользователя</param>
        /// <param name="permissionIds">Массив разрешённых PermissionID (например, new int[] {2, 3})</param>
        public User GetUserByNameAndPermissions(string teacherName, int[] permissionIds)
        {
            if (string.IsNullOrWhiteSpace(teacherName) || permissionIds == null || permissionIds.Length == 0)
                return null;

            SqlConnection connection = null;
            SqlDataReader reader = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                // ✅ ДИНАМИЧЕСКИ формируем условие IN (@id1, @id2, ...)
                string permissionCondition = string.Join(",", permissionIds.Select((id, index) => $"@p{index}"));
                string sql = $@"
            SELECT 
                u.UserID, 
                u.FullName, 
                u.PermissionID, 
                u.ClassID, 
                p.PermissionName
            FROM Users u
            JOIN Permissions p ON u.PermissionID = p.PermissionID
            WHERE u.FullName = @FullName 
              AND u.PermissionID IN ({permissionCondition})";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@FullName", teacherName.Trim());

                // ✅ Добавляем параметры для каждой роли
                for (int i = 0; i < permissionIds.Length; i++)
                {
                    cmd.Parameters.AddWithValue($"@p{i}", permissionIds[i]);
                }

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int ordUserId = reader.GetOrdinal("UserID");
                    int ordFullName = reader.GetOrdinal("FullName");
                    int ordPermissionId = reader.GetOrdinal("PermissionID");
                    int ordPermissionName = reader.GetOrdinal("PermissionName");
                    int ordClassId = reader.GetOrdinal("ClassID");

                    return new User
                    {
                        UserID = reader.GetInt32(ordUserId),
                        FullName = reader.GetString(ordFullName),
                        PermissionID = reader.GetInt32(ordPermissionId),
                        PermissionName = reader.GetString(ordPermissionName),
                        ClassID = reader.IsDBNull(ordClassId) ? null : (int?)reader.GetInt32(ordClassId)
                    };
                }

                return null;
            }
            finally
            {
                reader?.Dispose();
                connection?.Dispose();
            }
        }

        /// <summary>
        /// Только учителя (PermissionID = 2)
        /// </summary>
        public User GetTeacherByName(string teacherName)
        {
            return GetUserByNameAndPermissions(teacherName, new int[] { 2 });
        }

        public List<User> GetAllTeachers()
        {
            return UserController._userController.GetAllOfPredicate("u.PermissionID >= 2");
        }

        public List<User> GetAllStudents()
        {
            return UserController._userController.GetAllOfPredicate("u.PermissionID <= 1");
        }

        /// <summary>
        /// Учителя + Директор (PermissionID = 2, 3)
        /// </summary>
        public User GetTeacherOrDirectorByName(string teacherName)
        {
            return GetUserByNameAndPermissions(teacherName, new int[] { 2, 3 });
        }

        /// <summary>
        /// ID учителя по имени (совместимость)
        /// </summary>
        public int GetTeacherIdByName(string teacherName)
        {
            var teacher = GetTeacherByName(teacherName);
            return teacher?.UserID ?? 0;
        }
    }
}
