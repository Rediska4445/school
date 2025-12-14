using Microsoft.Data.SqlClient;
using school.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school
{
    public class AtterdanceController
    {
        public static AtterdanceController Instance = new AtterdanceController();

        // ✅ ДОБАВЬ в модель Attendance:
        public string SubjectName { get; set; } = "";
        public string StatusDisplay { get; set; } = "";

        public List<Attendance> GetStudentAttendance(int userId)
        {
            using (var conn = new SqlConnection(Form1.CONNECTION_STRING))
            {
                conn.Open();

                using (var cmd = new SqlCommand(@"
                    SELECT 
                        a.AttendanceID,           -- 0
                        a.AttendanceDate,         -- 1  
                        a.LessonDate,             -- 2
                        a.Present,                -- 3
                        a.ExcuseReason,           -- 4
                        a.Comment,                -- 5
                        ISNULL(s.SubjectName,'') AS SubjectName,  -- 6
                        CASE WHEN a.Present = 1 THEN '✓ Присутствует'
                            WHEN a.ExcuseReason = 1 THEN '⚠️ Уважительно'
                            ELSE '✗ Прогул' END AS StatusDisplay  -- 7
                    FROM Attendance a
                    LEFT JOIN Subjects s ON a.SubjectID = s.SubjectID
                    WHERE a.UserID = @UserID
                    ORDER BY a.AttendanceDate DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    var attendances = new List<Attendance>();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            attendances.Add(new Attendance
                            {
                                AttendanceID = reader.GetInt32(0),           // ✅ ИНДЕКС 0
                                AttendanceDate = reader.GetDateTime(1),      // ✅ ИНДЕКС 1
                                LessonDate = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),  // ✅ 2
                                Present = reader.GetBoolean(3),              // ✅ 3
                                ExcuseReason = reader.GetBoolean(4),         // ✅ 4
                                Comment = reader.IsDBNull(5) ? "" : reader.GetString(5),  // ✅ 5

                                // ✅ НОВЫЕ ПОЛЯ!
                                SubjectName = reader.IsDBNull(6) ? "" : reader.GetString(6),     // ✅ 6
                                StatusDisplay = reader.GetString(7)                                // ✅ 7
                            });
                        }
                    }

                    return attendances;
                }
            }
        }
    }
}
