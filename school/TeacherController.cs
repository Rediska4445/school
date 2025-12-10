using Microsoft.Data.SqlClient;
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
                        g.SubjectID,
                        s.SubjectName,
                        g.GradeValue,
                        g.TeacherID,
                        t.FullName AS TeacherFullName
                    FROM Grades g
                    JOIN Users stu ON g.StudentID = stu.UserID
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
                                Student = new User { FullName = reader.GetString(reader.GetOrdinal("StudentFullName")) },
                                SubjectID = reader.GetInt32(reader.GetOrdinal("SubjectID")),
                                Subject = new Subject { SubjectName = reader.GetString(reader.GetOrdinal("SubjectName")) },
                                GradeValue = reader.GetByte(reader.GetOrdinal("GradeValue")),
                                TeacherID = reader.GetInt32(reader.GetOrdinal("TeacherID")),
                                Teacher = new User { FullName = reader.GetString(reader.GetOrdinal("TeacherFullName")) }
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
