using Microsoft.Data.SqlClient;
using school.Models;
using System;
using System.Collections.Generic;

namespace school
{

    public class GradesController
    {
        private readonly string _connectionString;

        public static GradesController _controller = new GradesController(Form1.CONNECTION_STRING);

        public GradesController(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Вставка или обновление оценки
        public void InsertOrUpdateGrade(Grade grade)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                // Проверяем, есть ли уже оценка по дате, предмету, ученику
                SqlCommand checkCmd = new SqlCommand(@"
                SELECT GradeID FROM Grades 
                WHERE GradeDate = @GradeDate AND SubjectID = @SubjectID AND StudentID = @StudentID", connection);

                checkCmd.Parameters.AddWithValue("@GradeDate", grade.GradeDate.Date);
                checkCmd.Parameters.AddWithValue("@SubjectID", grade.SubjectID);
                checkCmd.Parameters.AddWithValue("@StudentID", grade.StudentID);

                object existingId = checkCmd.ExecuteScalar();

                if (existingId != null)
                {
                    // Обновить существующую оценку
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
                    // Вставить новую оценку
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

        // Получение оценки по предмету (объект), ученику (объект), дате
        public Grade GetGradeBySubjectStudentDate(Subject subject, User student, DateTime date)
        {
            return GetGradeByIds(subject.SubjectID, student.UserID, date);
        }

        // Получение оценки по названию предмета, логину ученика, дате
        public Grade GetGradeBySubjectNameStudentLoginDate(string subjectName, string studentLogin, DateTime date)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                // Получим SubjectID по имени предмета
                SqlCommand subjectCmd = new SqlCommand("SELECT SubjectID FROM Subjects WHERE SubjectName = @SubjectName", connection);
                subjectCmd.Parameters.AddWithValue("@SubjectName", subjectName);
                object subjectIdObj = subjectCmd.ExecuteScalar();
                if (subjectIdObj == null)
                    return null;
                int subjectId = (int)subjectIdObj;

                // Получим StudentID по полному имени
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

        // Вспомогательный метод получения оценки по IDs
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
