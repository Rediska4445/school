using System;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using school.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace school.Controllers
{
    /// <summary>
    /// [translate:Контроллер для работы с таблицей Homework]
    /// </summary>
    public class HomeworkController
    {
        public class HomeworkChange
        {
            public string Action { get; set; } // "EDIT", "ADD", "DELETE"
            public Homework Homework { get; set; } = new Homework();
        }

        private List<HomeworkChange> pendingChanges = new List<HomeworkChange>();

        private readonly string _connectionString;

        public static HomeworkController _homeworkController = new HomeworkController(Form1.CONNECTION_STRING);

        public HomeworkController(string connectionString)
        {
            _connectionString = connectionString ??
                Form1.CONNECTION_STRING;
        }

        /// <summary>
        /// Добавляет изменение в очередь
        /// </summary>
        public void AddHomeworkChange(string action, Homework homework)
        {
            if (UserController.CurrentUser.PermissionID <= 1) return;

            pendingChanges.Add(new HomeworkChange
            {
                Action = action,
                Homework = homework
            });
        }

        /// <summary>
        /// Выполняет все изменения из очереди и очищает её
        /// </summary>
        public int CommitHomeworkChanges()
        {
            if (pendingChanges.Count == 0) return 0;

            int processed = 0;
            try
            {
                foreach (var change in pendingChanges)
                {
                    switch (change.Action.ToUpper())
                    {
                        case "EDIT":
                        case "ADD":
                            InsertOrUpdateHomework(change.Homework);
                            processed++;
                            break;
                        case "DELETE":
                            if (DeleteHomework(change.Homework))
                                processed++;
                            break;
                    }
                }
                pendingChanges.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Commit error: {ex.Message}");
            }

            return processed;
        }

        /// <summary>
        /// Количество ожидающих изменений
        /// </summary>
        public int PendingChangesCount => pendingChanges.Count;

        /// <summary>
        /// [translate:Удаление домашнего задания]
        /// </summary>
        public bool DeleteHomework(Homework homework)
        {
            if (homework == null || homework.HomeworkID <= 0)
                return false;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // ✅ Проверка FK зависимостей (Grades ссылается на Homework? Нет, но проверяем существование)
                using (var cmd = new SqlCommand("DELETE FROM Homework WHERE HomeworkID = @HomeworkID", conn))
                {
                    cmd.Parameters.AddWithValue("@HomeworkID", homework.HomeworkID);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        /// <summary>
        /// [translate:Вставка новой домашки или обновление существующей (уникально по AssignmentDate+SubjectID+ClassID)]
        /// </summary>
        public int InsertOrUpdateHomework(Homework homework)
        {
            // ✅ ArgumentException для null
            if (homework == null)
                throw new ArgumentException("[translate:Домашка не может быть null]");

            // ✅ DataAnnotations ValidationException для Required/StringLength
            var validationContext = new ValidationContext(homework);
            Validator.ValidateObject(homework, validationContext, true);

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // ✅ ПРОВЕРКА уникальности: AssignmentDate + ClassID + SubjectID
                using (var checkCmd = new SqlCommand(@"
                    SELECT HomeworkID FROM Homework 
                    WHERE AssignmentDate = @Date AND ClassID = @ClassID AND SubjectID = @SubjectID", conn))
                {
                    checkCmd.Parameters.AddWithValue("@Date", homework.AssignmentDate.Date);
                    checkCmd.Parameters.AddWithValue("@ClassID", homework.ClassID);
                    checkCmd.Parameters.AddWithValue("@SubjectID", homework.SubjectID);

                    var existingIdObj = checkCmd.ExecuteScalar();
                    int existingId = existingIdObj != null ? (int)existingIdObj : 0;

                    if (existingId == 0)
                    {
                        // ✅ НОВОЕ ДЗ
                        using (var cmd = new SqlCommand(@"
                            INSERT INTO Homework (AssignmentDate, ClassID, SubjectID, Description, TeacherID)
                            OUTPUT INSERTED.HomeworkID
                            VALUES (@Date, @ClassID, @SubjectID, @Description, @TeacherID)", conn))
                        {
                            cmd.Parameters.AddWithValue("@Date", homework.AssignmentDate.Date);
                            cmd.Parameters.AddWithValue("@ClassID", homework.ClassID);
                            cmd.Parameters.AddWithValue("@SubjectID", homework.SubjectID);
                            cmd.Parameters.AddWithValue("@Description", homework.Description);
                            cmd.Parameters.AddWithValue("@TeacherID", homework.TeacherID);

                            int newId = (int)cmd.ExecuteScalar();
                            homework.HomeworkID = newId;
                            return newId;
                        }
                    }
                    else
                    {
                        // ✅ ОБНОВЛЕНИЕ существующего ДЗ
                        using (var cmd = new SqlCommand(@"
                            UPDATE Homework 
                            SET Description = @Description, TeacherID = @TeacherID
                            WHERE HomeworkID = @HomeworkID", conn))
                        {
                            cmd.Parameters.AddWithValue("@Description", homework.Description);
                            cmd.Parameters.AddWithValue("@TeacherID", homework.TeacherID);
                            cmd.Parameters.AddWithValue("@HomeworkID", existingId);
                            cmd.ExecuteNonQuery();
                            homework.HomeworkID = existingId;
                            return existingId;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// [translate:Получение домашки по предмету и дате (ClassID тоже учитывается)]
        /// </summary>
        public Homework GetHomeworkBySubjectAndDate(int subjectId, DateTime date, int classId)
        {
            if (subjectId <= 0 || classId <= 0) return null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                    SELECT HomeworkID, AssignmentDate, ClassID, SubjectID, Description, TeacherID 
                    FROM Homework 
                    WHERE SubjectID = @SubjectID AND AssignmentDate = @Date AND ClassID = @ClassID", conn))
                {
                    cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                    cmd.Parameters.AddWithValue("@Date", date.Date);
                    cmd.Parameters.AddWithValue("@ClassID", classId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Homework
                            {
                                HomeworkID = reader.GetInt32(0),
                                AssignmentDate = reader.GetDateTime(1),
                                ClassID = reader.GetInt32(2),
                                SubjectID = reader.GetInt32(3),
                                Description = reader.GetString(4),
                                TeacherID = reader.GetInt32(5)
                            };
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// [translate:Получение домашки по ID]
        /// </summary>
        public Homework GetHomeworkById(int homeworkId)
        {
            if (homeworkId <= 0) return null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                    SELECT HomeworkID, AssignmentDate, ClassID, SubjectID, Description, TeacherID 
                    FROM Homework WHERE HomeworkID = @HomeworkID", conn))
                {
                    cmd.Parameters.AddWithValue("@HomeworkID", homeworkId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Homework
                            {
                                HomeworkID = reader.GetInt32(0),
                                AssignmentDate = reader.GetDateTime(1),
                                ClassID = reader.GetInt32(2),
                                SubjectID = reader.GetInt32(3),
                                Description = reader.GetString(4),
                                TeacherID = reader.GetInt32(5)
                            };
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// [translate:Получение ДЗ за период для конкретного класса (с названиями классов/предметов/учителей)]
        /// </summary>
        /// <param name="classId">ID класса (например, 1 для 5А)</param>
        /// <param name="startDate">Начало периода</param>
        /// <param name="endDate">Конец периода</param>
        /// <returns>Список ДЗ с полными названиями</returns>
        public List<Homework> GetHomeworkForClassPeriod(int classId, DateTime startDate, DateTime endDate)
        {
            FileLogger.logger.Debug($"GetHomeworkForClassPeriod: classId={classId}, start={startDate:yyyy-MM-dd}, end={endDate:yyyy-MM-dd}");

            var homeworkList = new List<Homework>();
            int recordCount = 0;

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    FileLogger.logger.Debug("Connection created");
                    conn.Open();
                    FileLogger.logger.Debug("Connection opened");

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
                    t.FullName AS TeacherName
                FROM Homework h
                JOIN Classes c ON h.ClassID = c.ClassID
                JOIN Subjects s ON h.SubjectID = s.SubjectID
                JOIN Users t ON h.TeacherID = t.UserID
                WHERE h.ClassID = @ClassID
                AND h.AssignmentDate BETWEEN @StartDate AND @EndDate
                ORDER BY h.AssignmentDate, s.SubjectName", conn))
                    {
                        FileLogger.logger.Debug("Command created");

                        cmd.Parameters.AddWithValue("@ClassID", classId);
                        cmd.Parameters.AddWithValue("@StartDate", startDate.Date);
                        cmd.Parameters.AddWithValue("@EndDate", endDate.Date);
                        FileLogger.logger.Debug($"Parameters set: ClassID={classId}");

                        using (var reader = cmd.ExecuteReader())
                        {
                            FileLogger.logger.Debug("Reader created");

                            while (reader.Read())
                            {
                                recordCount++;
                                FileLogger.logger.Debug($"Processing record #{recordCount}");

                                int homeworkIDIdx = reader.GetOrdinal("HomeworkID");
                                int assignmentDateIdx = reader.GetOrdinal("AssignmentDate");
                                int classIDIdx = reader.GetOrdinal("ClassID");
                                int classNameIdx = reader.GetOrdinal("ClassName");
                                int subjectIDIdx = reader.GetOrdinal("SubjectID");
                                int subjectNameIdx = reader.GetOrdinal("SubjectName");
                                int descriptionIdx = reader.GetOrdinal("Description");
                                int teacherIDIdx = reader.GetOrdinal("TeacherID");
                                int teacherNameIdx = reader.GetOrdinal("TeacherName");

                                FileLogger.logger.Debug($"HomeworkID[{homeworkIDIdx}]: {reader.GetInt32(homeworkIDIdx)}");
                                FileLogger.logger.Debug($"ClassName[{classNameIdx}]: DBNull={reader.IsDBNull(classNameIdx)}");
                                FileLogger.logger.Debug($"SubjectName[{subjectNameIdx}]: DBNull={reader.IsDBNull(subjectNameIdx)}");
                                FileLogger.logger.Debug($"TeacherName[{teacherNameIdx}]: DBNull={reader.IsDBNull(teacherNameIdx)}");
                                FileLogger.logger.Debug($"Description[{descriptionIdx}]: DBNull={reader.IsDBNull(descriptionIdx)}");

                                var homework = new Homework
                                {
                                    HomeworkID = reader.GetInt32(homeworkIDIdx),
                                    AssignmentDate = reader.GetDateTime(assignmentDateIdx),
                                    ClassID = reader.GetInt32(classIDIdx),
                                    Class = new Class
                                    {
                                        ClassID = reader.GetInt32(classIDIdx),
                                        ClassName = reader.IsDBNull(classNameIdx) ? "" : reader.GetString(classNameIdx)
                                    },
                                    SubjectID = reader.GetInt32(subjectIDIdx),
                                    Subject = new Subject
                                    {
                                        SubjectID = reader.GetInt32(subjectIDIdx),
                                        SubjectName = reader.IsDBNull(subjectNameIdx) ? "" : reader.GetString(subjectNameIdx)
                                    },
                                    Description = reader.IsDBNull(descriptionIdx) ? "" : reader.GetString(descriptionIdx),
                                    TeacherID = reader.GetInt32(teacherIDIdx),
                                    Teacher = new User
                                    {
                                        UserID = reader.GetInt32(teacherIDIdx),
                                        FullName = reader.IsDBNull(teacherNameIdx) ? "" : reader.GetString(teacherNameIdx)
                                    }
                                };

                                FileLogger.logger.Debug("Homework created OK");
                                homeworkList.Add(homework);
                            }

                            FileLogger.logger.Debug($"Reader completed. Processed {recordCount} records");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"GetHomeworkForClassPeriod FAILED: {ex.Message}");
                FileLogger.logger.Error($"Stack trace: {ex.StackTrace}");
                throw;
            }

            FileLogger.logger.Debug($"Method completed. Returned {homeworkList.Count} records");
            return homeworkList;
        }

        /// <summary>
        /// [translate:Получение всех домашних заданий за период]
        /// </summary>
        public List<Homework> GetHomeworkForPeriod(DateTime startDate, DateTime endDate)
        {
            var homeworkList = new List<Homework>();
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                using (var cmd = new SqlCommand(@"
                    SELECT HomeworkID, AssignmentDate, ClassID, SubjectID, Description, TeacherID
                    FROM Homework
                    WHERE AssignmentDate BETWEEN @StartDate AND @EndDate
                    ORDER BY AssignmentDate, ClassID, SubjectID", connection))
                {
                    cmd.Parameters.AddWithValue("@StartDate", startDate.Date);
                    cmd.Parameters.AddWithValue("@EndDate", endDate.Date);

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var homework = new Homework
                        {
                            HomeworkID = reader.GetInt32(reader.GetOrdinal("HomeworkID")),
                            AssignmentDate = reader.GetDateTime(reader.GetOrdinal("AssignmentDate")),
                            ClassID = reader.GetInt32(reader.GetOrdinal("ClassID")),
                            SubjectID = reader.GetInt32(reader.GetOrdinal("SubjectID")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            TeacherID = reader.GetInt32(reader.GetOrdinal("TeacherID"))
                        };
                        homeworkList.Add(homework);
                    }
                }

                return homeworkList;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                if (connection != null)
                    connection.Dispose();
            }
        }
    }
}