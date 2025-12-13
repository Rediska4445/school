using Microsoft.Data.SqlClient;
using school.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school
{
    public class StatisticsController
    {
        public static StatisticsController _controller = new StatisticsController();

        public string CONNECTION_STRING = Form1.CONNECTION_STRING;

        /// <summary>
        /// Возвращает статистику ученика за период в формате Ключ=Значение
        /// </summary>
        public Dictionary<string, string> GetStudentStatistics(User student)
        {
            int studentId = student.UserID;

            var statistics = new Dictionary<string, string>();

            var connection = new SqlConnection(Form1.CONNECTION_STRING);
            var command = new SqlCommand(@"
        SELECT 
            s.SubjectName AS SubjectKey,
            FORMAT(AVG(CAST(g.GradeValue AS FLOAT)), 'N2') AS AverageValue
        FROM Grades g
        INNER JOIN Subjects s ON g.SubjectID = s.SubjectID
        WHERE g.StudentID = @StudentID
        GROUP BY s.SubjectID, s.SubjectName", connection);

            command.Parameters.AddWithValue("@StudentID", studentId);

            connection.Open();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                string key = "Ср. балл по " + reader.GetString(0); 
                string value = reader.GetString(1);
                statistics[key] = value;
            }

            reader.Close();
            connection.Close();

            return statistics;
        }

        public Dictionary<string, string> GetClassStatistics(Class classObj)
        {
            var statistics = new Dictionary<string, string>();

            var connection = new SqlConnection(Form1.CONNECTION_STRING);
            var command = new SqlCommand(@"
            SELECT 
                CONCAT(c.ClassName, ' - ', s.SubjectName, ' (учеников: ', 
                       COUNT(DISTINCT u.UserID), ', ср: ', 
                       FORMAT(AVG(CAST(g.GradeValue AS FLOAT)), 'N2'), 
                       ', оценок: ', COUNT(g.GradeID), 
                       ', мин: ', MIN(g.GradeValue), ', макс: ', MAX(g.GradeValue), ')') AS StatLine
            FROM Classes c
            INNER JOIN Users u ON c.ClassID = u.ClassID
            INNER JOIN Permissions p ON u.PermissionID = p.PermissionID
            LEFT JOIN Subjects s ON 1=1
            LEFT JOIN Grades g ON g.StudentID = u.UserID AND g.SubjectID = s.SubjectID
            WHERE c.ClassID = @ClassID
              AND p.PermissionName = N'Ученик'
            GROUP BY c.ClassID, c.ClassName, s.SubjectID, s.SubjectName
            HAVING COUNT(g.GradeID) > 0
            ORDER BY s.SubjectName", connection);

            command.Parameters.AddWithValue("@ClassID", classObj.ClassID);

            connection.Open();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                string statLine = reader.GetString(0);
                statistics[statLine] = statLine;
            }

            reader.Close();
            connection.Close();

            return statistics;
        }

        public Dictionary<string, string[]> GetClassStatisticsFull(Class classObj)
        {
            var statistics = new Dictionary<string, string[]>();

            var connection = new SqlConnection(Form1.CONNECTION_STRING);
            var command = new SqlCommand(@"
                SELECT 
                    CONCAT(c.ClassName, ' - ', s.SubjectName) AS StatKey,
                    CAST(COUNT(DISTINCT u.UserID) AS VARCHAR(10)) AS StudentsCount,
                    FORMAT(AVG(CAST(g.GradeValue AS FLOAT)), 'N2') AS AverageGrade,
                    CAST(COUNT(g.GradeID) AS VARCHAR(10)) AS TotalGrades,
                    CAST(MIN(g.GradeValue) AS VARCHAR(2)) AS MinGrade,
                    CAST(MAX(g.GradeValue) AS VARCHAR(2)) AS MaxGrade
                FROM Classes c
                INNER JOIN Users u ON c.ClassID = u.ClassID
                INNER JOIN Permissions p ON u.PermissionID = p.PermissionID
                LEFT JOIN Subjects s ON 1=1
                LEFT JOIN Grades g ON g.StudentID = u.UserID AND g.SubjectID = s.SubjectID
                WHERE c.ClassID = @ClassID
                  AND p.PermissionName = N'Ученик'
                GROUP BY c.ClassID, c.ClassName, s.SubjectID, s.SubjectName
                HAVING COUNT(g.GradeID) > 0
                ORDER BY s.SubjectName", connection);

            command.Parameters.AddWithValue("@ClassID", classObj.ClassID);

            connection.Open();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                string key = reader.GetString(0);
                string[] values = {
                    reader.GetString(1), // Учеников
                    reader.GetString(2), // Средний
                    reader.GetString(3), // Оценок
                    reader.GetString(4), // Мин
                    reader.GetString(5)  // Макс
                };
                statistics[key] = values;
            }

            reader.Close();
            connection.Close();
            return statistics;
        }
    }
}
