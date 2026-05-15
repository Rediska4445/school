using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school
{
    public class QuarterGradesController
    {
        public static QuarterGradesController _controller = new QuarterGradesController();

        public string ConnectionString = Form1.CONNECTION_STRING;

        public void SaveOrUpdateQuarterGrades(
            int studentId,
            int subjectId,
            int? q1,
            int? q2,
            int? q3,
            int? q4)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                const string selectSql = @"
                SELECT QuarterGradeID
                FROM QuarterGrades
                WHERE StudentID = @StudentID
                  AND SubjectID = @SubjectID";

                using (var selectCmd = new SqlCommand(selectSql, connection))
                {
                    selectCmd.Parameters.AddWithValue("@StudentID", studentId);
                    selectCmd.Parameters.AddWithValue("@SubjectID", subjectId);

                    var result = selectCmd.ExecuteScalar();
                    int quarterGradeId = result != null ? (int)result : 0;

                    if (quarterGradeId > 0)
                    {
                        UpdateQuarterGrades(connection, quarterGradeId, q1, q2, q3, q4);
                    }
                    else
                    {
                        InsertQuarterGrades(connection, studentId, subjectId, q1, q2, q3, q4);
                    }
                }
            }
        }

        private void InsertQuarterGrades(
            SqlConnection connection,
            int studentId,
            int subjectId,
            int? q1,
            int? q2,
            int? q3,
            int? q4)
        {
            const string sql = @"
            INSERT INTO QuarterGrades (StudentID, SubjectID, Quarter1Grade, Quarter2Grade, Quarter3Grade, Quarter4Grade)
            VALUES (@StudentID, @SubjectID, @Q1, @Q2, @Q3, @Q4)";

            using (var cmd = new SqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                cmd.Parameters.AddWithValue("@Q1", q1 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Q2", q2 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Q3", q3 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Q4", q4 ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateQuarterGrades(
            SqlConnection connection,
            int quarterGradeId,
            int? q1,
            int? q2,
            int? q3,
            int? q4)
        {
            const string sql = @"
            UPDATE QuarterGrades
            SET Quarter1Grade = @Q1,
                Quarter2Grade = @Q2,
                Quarter3Grade = @Q3,
                Quarter4Grade = @Q4
            WHERE QuarterGradeID = @QuarterGradeID";

            using (var cmd = new SqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@QuarterGradeID", quarterGradeId);
                cmd.Parameters.AddWithValue("@Q1", q1 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Q2", q2 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Q3", q3 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Q4", q4 ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();
            }
        }

        public List<QuarterGrade> GetQuarterGradesForStudent(int studentId)
        {
            var result = new List<QuarterGrade>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                const string sql = @"
            SELECT QuarterGradeID, StudentID, SubjectID, Quarter1Grade, Quarter2Grade, Quarter3Grade, Quarter4Grade
            FROM QuarterGrades
            WHERE StudentID = @StudentID";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idOrdinal = reader.GetOrdinal("QuarterGradeID");
                            int studentIdOrdinal = reader.GetOrdinal("StudentID");
                            int subjectIdOrdinal = reader.GetOrdinal("SubjectID");
                            int q1Ordinal = reader.GetOrdinal("Quarter1Grade");
                            int q2Ordinal = reader.GetOrdinal("Quarter2Grade");
                            int q3Ordinal = reader.GetOrdinal("Quarter3Grade");
                            int q4Ordinal = reader.GetOrdinal("Quarter4Grade");

                            result.Add(new QuarterGrade
                            {
                                QuarterGradeID = reader.GetInt32(idOrdinal),
                                StudentID = reader.GetInt32(studentIdOrdinal),
                                SubjectID = reader.GetInt32(subjectIdOrdinal),
                                Quarter1Grade = reader.IsDBNull(q1Ordinal) ? (byte?)null : reader.GetByte(q1Ordinal),
                                Quarter2Grade = reader.IsDBNull(q2Ordinal) ? (byte?)null : reader.GetByte(q2Ordinal),
                                Quarter3Grade = reader.IsDBNull(q3Ordinal) ? (byte?)null : reader.GetByte(q3Ordinal),
                                Quarter4Grade = reader.IsDBNull(q4Ordinal) ? (byte?)null : reader.GetByte(q4Ordinal)
                            });
                        }
                    }
                }
            }

            return result;
        }
    }
}