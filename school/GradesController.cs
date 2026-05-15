using Microsoft.Data.SqlClient;
using school.Controllers;
using school.Models;
using System;
using System.Collections.Generic;

namespace school
{
    public class GradesController
    {
        private readonly string _connectionString;

        public static GradesController _controller = new GradesController(Form1.CONNECTION_STRING);

        public class GradeChange
        {
            public string Action { get; set; } 
            public Grade Grade { get; set; } = new Grade();
        }

        private List<GradeChange> pendingChanges = new List<GradeChange>();

        /// <summary>
        /// Добавляет изменение оценки в очередь
        /// </summary>
        public void AddGradeChange(string action, Grade grade)
        {
            if (UserController.CurrentUser.PermissionID <= 1) return;

            pendingChanges.Add(new GradeChange
            {
                Action = action,
                Grade = grade
            });
        }

        /// <summary>
        /// Выполняет все изменения оценок и очищает очередь
        /// </summary>
        public int CommitGradeChanges()
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
                            InsertOrUpdateGrade(change.Grade);
                            processed++;
                            break;
                        case "DELETE":
                            DeleteGrade(change.Grade);
                            processed++;
                            break;
                    }
                }
                pendingChanges.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Commit grades error: {ex.Message}");
            }
            return processed;
        }

        /// <summary>
        /// Количество ожидающих изменений оценок
        /// </summary>
        public int PendingChangesCount => pendingChanges.Count;

        public GradesController(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Grade[] GetAvailableGrades()
        {
            return new Grade[] {
                new Grade()
                {
                    GradeValue = 1,
                },
                new Grade()
                {
                    GradeValue = 2,
                },
                new Grade()
                {
                    GradeValue = 3,
                },
                new Grade()
                {
                    GradeValue = 4,
                },
                new Grade()
                {
                    GradeValue = 5,
                }
            };
        }

        public void InsertOrUpdateGrade(Grade grade)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                SqlCommand checkCmd = new SqlCommand(@"
                SELECT GradeID FROM Grades 
                WHERE GradeDate = @GradeDate AND SubjectID = @SubjectID AND StudentID = @StudentID", connection);

                checkCmd.Parameters.AddWithValue("@GradeDate", grade.GradeDate.Date);
                checkCmd.Parameters.AddWithValue("@SubjectID", grade.SubjectID);
                checkCmd.Parameters.AddWithValue("@StudentID", grade.StudentID);

                object existingId = checkCmd.ExecuteScalar();

                if (existingId != null)
                {
                    SqlCommand updateCmd = new SqlCommand(@"
                    UPDATE Grades SET GradeValue = @GradeValue, TeacherID = @TeacherID 
                    WHERE GradeID = @GradeID", connection);
                    updateCmd.Parameters.AddWithValue("@GradeValue", grade.GradeValue);
                    updateCmd.Parameters.AddWithValue("@TeacherID", grade.TeacherID);
                    updateCmd.Parameters.AddWithValue("@GradeID", (int)existingId);

                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand insertCmd = new SqlCommand(@"
                    INSERT INTO Grades (GradeDate, StudentID, SubjectID, GradeValue, TeacherID)
                    VALUES (@GradeDate, @StudentID, @SubjectID, @GradeValue, @TeacherID)", connection);

                    insertCmd.Parameters.AddWithValue("@GradeDate", grade.GradeDate.Date);
                    insertCmd.Parameters.AddWithValue("@StudentID", grade.StudentID);
                    insertCmd.Parameters.AddWithValue("@SubjectID", grade.SubjectID);
                    insertCmd.Parameters.AddWithValue("@GradeValue", grade.GradeValue);
                    insertCmd.Parameters.AddWithValue("@TeacherID", grade.TeacherID);

                    insertCmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }
        }

        // Удалить оценку (по GradeID)
        public void DeleteGrade(Grade grade)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM Grades WHERE GradeID = @GradeID", connection);
                cmd.Parameters.AddWithValue("@GradeID", grade.GradeID);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }
        }

        public Grade GetGradeBySubjectStudentDate(Subject subject, User student, DateTime date)
        {
            return GetGradeByIds(subject.SubjectID, student.UserID, date);
        }

        public Grade GetGradeBySubjectNameStudentLoginDate(string subjectName, string studentLogin, DateTime date)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                SqlCommand subjectCmd = new SqlCommand("SELECT SubjectID FROM Subjects WHERE SubjectName = @SubjectName", connection);
                subjectCmd.Parameters.AddWithValue("@SubjectName", subjectName);
                object subjectIdObj = subjectCmd.ExecuteScalar();
                if (subjectIdObj == null)
                    return null;
                int subjectId = (int)subjectIdObj;

                SqlCommand studentCmd = new SqlCommand("SELECT UserID FROM Users WHERE FullName = @FullName AND Role = N'Ученик'", connection);
                studentCmd.Parameters.AddWithValue("@FullName", studentLogin);
                object studentIdObj = studentCmd.ExecuteScalar();
                if (studentIdObj == null)
                    return null;
                int studentId = (int)studentIdObj;

                return GetGradeByIds(subjectId, studentId, date);
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }
        }

        private Grade GetGradeByIds(int subjectId, int studentId, DateTime date)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                SqlCommand cmd = new SqlCommand(@"
                SELECT GradeID, GradeDate, StudentID, SubjectID, GradeValue, TeacherID 
                FROM Grades 
                WHERE SubjectID = @SubjectID AND StudentID = @StudentID AND GradeDate = @GradeDate", connection);

                cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@GradeDate", date.Date);

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Grade
                    {
                        GradeID = reader.GetInt32(reader.GetOrdinal("GradeID")),
                        GradeDate = reader.GetDateTime(reader.GetOrdinal("GradeDate")),
                        StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                        SubjectID = reader.GetInt32(reader.GetOrdinal("SubjectID")),
                        GradeValue = (byte)reader.GetByte(reader.GetOrdinal("GradeValue")),
                        TeacherID = reader.GetInt32(reader.GetOrdinal("TeacherID"))
                    };
                }

                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                if (connection != null)
                    connection.Dispose();
            }
        }

        public List<QuarterGradeRow> GetQuarterGradesByStudentId(int studentId)
        {
            if (studentId <= 0)
                return new List<QuarterGradeRow>();

            const string sql = @"
                SELECT
                    s.SubjectID,
                    s.SubjectName,
                    qg.Quarter1Grade,
                    qg.Quarter2Grade,
                    qg.Quarter3Grade,
                    qg.Quarter4Grade
                FROM QuarterGrades qg
                JOIN Subjects s ON s.SubjectID = qg.SubjectID
                WHERE qg.StudentID = @StudentID
                ORDER BY s.SubjectName";

            var result = new List<QuarterGradeRow>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        int subjectIdOrdinal = reader.GetOrdinal("SubjectID");
                        int subjectNameOrdinal = reader.GetOrdinal("SubjectName");
                        int q1Ordinal = reader.GetOrdinal("Quarter1Grade");
                        int q2Ordinal = reader.GetOrdinal("Quarter2Grade");
                        int q3Ordinal = reader.GetOrdinal("Quarter3Grade");
                        int q4Ordinal = reader.GetOrdinal("Quarter4Grade");

                        while (reader.Read())
                        {
                            var row = new QuarterGradeRow
                            {
                                SubjectID = reader.GetInt32(subjectIdOrdinal),
                                SubjectName = reader.GetString(subjectNameOrdinal),
                                Quarter1Grade = reader.IsDBNull(q1Ordinal) ? (int?)null : reader.GetInt32(q1Ordinal),
                                Quarter2Grade = reader.IsDBNull(q2Ordinal) ? (int?)null : reader.GetInt32(q2Ordinal),
                                Quarter3Grade = reader.IsDBNull(q3Ordinal) ? (int?)null : reader.GetInt32(q3Ordinal),
                                Quarter4Grade = reader.IsDBNull(q4Ordinal) ? (int?)null : reader.GetInt32(q4Ordinal)
                            };

                            result.Add(row);
                        }
                    }
                }
            }

            return result;
        }

        public List<Grade> GetGradesForStudentPeriod(int studentId, DateTime startDate, DateTime endDate)
        {
            List<Grade> grades = new List<Grade>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(@"
                    SELECT 
                        g.GradeID, g.GradeDate, g.StudentID, g.SubjectID, g.GradeValue, g.TeacherID,
                        s.SubjectName, t.FullName AS TeacherName
                    FROM Grades g
                    JOIN Subjects s ON g.SubjectID = s.SubjectID
                    JOIN Users t ON g.TeacherID = t.UserID
                    WHERE g.StudentID = @StudentID
                        AND g.GradeDate BETWEEN @StartDate AND @EndDate
                    ORDER BY g.GradeDate, s.SubjectName", connection))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@StartDate", startDate.Date);
                    cmd.Parameters.AddWithValue("@EndDate", endDate.Date);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var grade = new Grade
                            {
                                GradeID = reader.GetInt32(reader.GetOrdinal("GradeID")),
                                GradeDate = reader.GetDateTime(reader.GetOrdinal("GradeDate")),
                                StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                                SubjectID = reader.GetInt32(reader.GetOrdinal("SubjectID")),
                                Subject = new Subject
                                {
                                    SubjectID = reader.GetInt32(reader.GetOrdinal("SubjectID")),
                                    SubjectName = reader.GetString(reader.GetOrdinal("SubjectName"))
                                },
                                GradeValue = reader.GetByte(reader.GetOrdinal("GradeValue")),
                                TeacherID = reader.GetInt32(reader.GetOrdinal("TeacherID")),
                                Teacher = new User
                                {
                                    UserID = reader.GetInt32(reader.GetOrdinal("TeacherID")),
                                    FullName = reader.GetString(reader.GetOrdinal("TeacherName"))
                                }
                            };
                            grades.Add(grade);
                        }

                    }
                }
            }
            return grades;
        }

        public List<Grade> GetGradesForPeriod(DateTime startDate, DateTime endDate)
        {
            List<Grade> grades = new List<Grade>();
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                SqlCommand cmd = new SqlCommand(@"
                SELECT 
                    g.GradeID, g.GradeDate, g.StudentID, g.SubjectID, g.GradeValue, g.TeacherID
                FROM Grades g
                WHERE g.GradeDate BETWEEN @StartDate AND @EndDate
                ORDER BY g.GradeDate, g.StudentID, g.SubjectID", connection);

                cmd.Parameters.AddWithValue("@StartDate", startDate.Date);
                cmd.Parameters.AddWithValue("@EndDate", endDate.Date);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Grade grade = new Grade
                    {
                        GradeID = reader.GetInt32(reader.GetOrdinal("GradeID")),
                        GradeDate = reader.GetDateTime(reader.GetOrdinal("GradeDate")),
                        StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                        SubjectID = reader.GetInt32(reader.GetOrdinal("SubjectID")),
                        GradeValue = (byte)reader.GetByte(reader.GetOrdinal("GradeValue")),
                        TeacherID = reader.GetInt32(reader.GetOrdinal("TeacherID"))
                    };
                    grades.Add(grade);
                }

                return grades;
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
