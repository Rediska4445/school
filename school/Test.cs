using NUnit.Framework;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace school
{
    public abstract class Test
    {
        protected string TestConnectionString { get; private set; }
        protected string OriginalConnectionString { get; private set; }

        [OneTimeSetUp]
        public void BaseOneTimeSetUp()
        {
            OriginalConnectionString = Form1.CONNECTION_STRING;
            TestConnectionString = @"Server=localhost\SQLEXPRESS;Database=schoolIntegrateTestDb;Integrated Security=True;TrustServerCertificate=True;";

            CreateTestDatabase();
            Form1.CONNECTION_STRING = TestConnectionString;
        }

        private void CreateTestDatabase()
        {
            Controller.sqlController.PrepareDatabase(
                @"Server=localhost\SQLEXPRESS;Database=master;Integrated Security=True;TrustServerCertificate=True;",
                "schoolIntegrateTestDb");
        }

        [SetUp]
        public void BaseSetUp()
        {
            ClearAllTables();
        }

        private void ClearAllTables()
        {
            using (var conn = new SqlConnection(TestConnectionString))
            {
                conn.Open();

                var deleteCommands = new List<string>
                {
                    "DELETE FROM Attendance",
                    "DELETE FROM Grades",
                    "DELETE FROM Homework",
                    "DELETE FROM Schedule",
                    "DELETE FROM TeacherSubjects",
                    "DELETE FROM Events",
                    "DELETE FROM Users",
                    "DELETE FROM Permissions",
                    "DELETE FROM Subjects",
                    "DELETE FROM Classes"
                };

                foreach (var cmdText in deleteCommands)
                {
                    try
                    {
                        new SqlCommand(cmdText, conn).ExecuteNonQuery();
                    }
                    catch (SqlException ex) when (ex.Message.Contains("Invalid object name"))
                    {
                        // Таблица не существует - игнорируем
                        continue;
                    }
                }
            }
        }
    }
}
