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

        public class StudentGradeStats
        {
            public string StudentName { get; set; } = "";
            public string AvgGrade { get; set; } = "0.00";
            public string TotalGrades { get; set; } = "0";
            public string MinGrade { get; set; } = "0";
            public string MaxGrade { get; set; } = "0";
            public Dictionary<string, string> SubjectGrades { get; set; } = new Dictionary<string, string>();
        }

        public Dictionary<string, StudentGradeStats> GetClassStudentStatisticsFull(int classId, DateTime startDate, DateTime endDate)
        {
            var statistics = new Dictionary<string, StudentGradeStats>();

            FileLogger.logger.Debug($"🔍 Отчет успеваемости: ClassID={classId}, период {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}");

            using (var connection = new SqlConnection(Form1.CONNECTION_STRING))
            {
                connection.Open();
                FileLogger.logger.Debug("✅ Соединение открыто");

                // 1. Ученики класса
                using (var cmdStudents = new SqlCommand(@"
            SELECT 
                u.UserID,
                u.FullName AS StudentName,
                ISNULL(FORMAT(AVG(CAST(g.GradeValue AS FLOAT)), 'N2'), '0.00') AS AvgGrade,
                ISNULL(COUNT(g.GradeID), 0) AS TotalGrades,
                ISNULL(MIN(g.GradeValue), 0) AS MinGrade,
                ISNULL(MAX(g.GradeValue), 0) AS MaxGrade
            FROM Users u 
            LEFT JOIN Grades g ON u.UserID = g.StudentID 
                AND g.GradeDate BETWEEN @StartDate AND @EndDate
            INNER JOIN Permissions p ON u.PermissionID = p.PermissionID
            WHERE u.ClassID = @ClassId 
                AND p.PermissionName = N'Ученик'
            GROUP BY u.UserID, u.FullName
            ORDER BY u.FullName", connection))
                {
                    cmdStudents.Parameters.AddWithValue("@ClassId", classId);
                    cmdStudents.Parameters.AddWithValue("@StartDate", startDate);
                    cmdStudents.Parameters.AddWithValue("@EndDate", endDate);

                    FileLogger.logger.Debug("📊 Запрос учеников выполнен");
                    using (var reader = cmdStudents.ExecuteReader())
                    {
                        int studentCount = 0;
                        while (reader.Read())
                        {
                            studentCount++;
                            var stats = new StudentGradeStats
                            {
                                StudentName = reader["StudentName"]?.ToString() ?? "",
                                AvgGrade = reader["AvgGrade"]?.ToString() ?? "0.00",
                                TotalGrades = reader["TotalGrades"]?.ToString() ?? "0",
                                MinGrade = reader["MinGrade"]?.ToString() ?? "0",
                                MaxGrade = reader["MaxGrade"]?.ToString() ?? "0"
                            };

                            statistics[stats.StudentName] = stats;
                            FileLogger.logger.Debug($"👤 Добавлен ученик: {stats.StudentName} | Ср={stats.AvgGrade} | Оценок={stats.TotalGrades}");
                        }
                        FileLogger.logger.Debug($"📈 Найдено учеников: {studentCount}");
                    }
                }

                // 2. Оценки по предметам
                using (var cmdSubjects = new SqlCommand(@"
            SELECT 
                u.FullName AS StudentName,
                s.SubjectName AS SubjectKey,
                FORMAT(AVG(CAST(g.GradeValue AS FLOAT)), 'N2') AS AverageValue
            FROM Users u 
            INNER JOIN Grades g ON u.UserID = g.StudentID 
                AND g.GradeDate BETWEEN @StartDate AND @EndDate
            INNER JOIN Subjects s ON g.SubjectID = s.SubjectID
            INNER JOIN Permissions p ON u.PermissionID = p.PermissionID
            WHERE u.ClassID = @ClassId 
                AND p.PermissionName = N'Ученик'
            GROUP BY u.UserID, u.FullName, s.SubjectID, s.SubjectName
            ORDER BY u.FullName, s.SubjectName", connection))
                {
                    cmdSubjects.Parameters.AddWithValue("@ClassId", classId);
                    cmdSubjects.Parameters.AddWithValue("@StartDate", startDate);
                    cmdSubjects.Parameters.AddWithValue("@EndDate", endDate);

                    FileLogger.logger.Debug("📚 Запрос предметов выполнен");
                    using (var reader = cmdSubjects.ExecuteReader())
                    {
                        int subjectCount = 0;
                        while (reader.Read())
                        {
                            string studentName = reader["StudentName"]?.ToString() ?? "";
                            string subjectKey = reader["SubjectKey"]?.ToString() ?? "";
                            string avgValue = reader["AverageValue"]?.ToString() ?? "0.00";

                            if (statistics.ContainsKey(studentName))
                            {
                                statistics[studentName].SubjectGrades[subjectKey] = avgValue;
                                subjectCount++;
                                FileLogger.logger.Debug($"📖 {studentName}: {subjectKey} = {avgValue}");
                            }
                        }
                        FileLogger.logger.Debug($"📖 Найдено предметов: {subjectCount}");
                    }
                }
            }

            FileLogger.logger.Debug($"✅ ИТОГО: {statistics.Count} учеников в словаре");
            return statistics;
        }

        public Dictionary<string, Dictionary<string, string>> GetClassMatrixStatistics(int classId, DateTime startDate, DateTime endDate)
        {
            var matrix = new Dictionary<string, Dictionary<string, string>>();

            using (var conn = new SqlConnection(Form1.CONNECTION_STRING))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
            SELECT 
                u.FullName,
                ISNULL(s.SubjectName, 'Ср.балл') AS SubjectName,
                FORMAT(AVG(CAST(g.GradeValue AS FLOAT)), 'N2') AS AvgGrade
            FROM Users u 
            INNER JOIN Permissions p ON u.PermissionID = p.PermissionID
            LEFT JOIN Grades g ON u.UserID = g.StudentID AND g.GradeDate BETWEEN @StartDate AND @EndDate
            LEFT JOIN Subjects s ON g.SubjectID = s.SubjectID
            WHERE u.ClassID = @ClassId AND p.PermissionName = N'Ученик'
            GROUP BY u.UserID, u.FullName, s.SubjectID, s.SubjectName
            HAVING COUNT(g.GradeID) > 0 OR s.SubjectID IS NULL
            ORDER BY u.FullName, s.SubjectName", conn))
                {
                    cmd.Parameters.AddWithValue("@ClassId", classId);
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string student = reader.GetString(0);
                            string subject = reader.GetString(1);
                            string avg = reader.IsDBNull(2) ? "0.00" : reader.GetString(2);

                            if (!matrix.ContainsKey(student))
                                matrix[student] = new Dictionary<string, string>();

                            matrix[student][subject] = avg;
                        }
                    }
                }
            }
            return matrix;
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
