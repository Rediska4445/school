using school.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school
{
    public class RegistrationApplicationController
    {
        public static RegistrationApplicationController _controller = new RegistrationApplicationController(Form1.CONNECTION_STRING);

        private readonly string _connectionString;

        public RegistrationApplicationController(string connection)
        {
            this._connectionString = connection;
        }

        public RegistrationApplication Create(string fullName, string passwordHash, int permissionID, int? classID, byte? age, string telephone)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"
                    INSERT INTO RegistrationApplications (FullName, PasswordHash, PermissionID, ClassID, Age, Telephone)
                    VALUES (@FullName, @PasswordHash, @PermissionID, @ClassID, @Age, @Telephone);
                    SELECT SCOPE_IDENTITY();", connection);

                command.Parameters.AddWithValue("@FullName", fullName);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                command.Parameters.AddWithValue("@PermissionID", permissionID);
                command.Parameters.AddWithValue("@ClassID", (object)classID ?? DBNull.Value);
                command.Parameters.AddWithValue("@Age", (object)age ?? DBNull.Value);
                command.Parameters.AddWithValue("@Telephone", (object)telephone ?? DBNull.Value);

                connection.Open();
                var applicationID = Convert.ToInt32(command.ExecuteScalar());

                return new RegistrationApplication
                {
                    ApplicationID = applicationID,
                    FullName = fullName,
                    PasswordHash = passwordHash,
                    PermissionID = permissionID,
                    ClassID = classID,
                    Age = age,
                    Telephone = telephone,
                    ApplicationDate = DateTime.Now,
                    IsApproved = false
                };
            }
        }

        public List<RegistrationApplication> GetAll()
        {
            var applications = new List<RegistrationApplication>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"
                    SELECT ApplicationID, FullName, PasswordHash, PermissionID, ClassID, Age, Telephone, ApplicationDate, IsApproved
                    FROM RegistrationApplications
                    ORDER BY ApplicationDate DESC", connection);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        applications.Add(new RegistrationApplication
                        {
                            ApplicationID = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            PasswordHash = reader.GetString(2),
                            PermissionID = reader.GetInt32(3),
                            ClassID = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                            Age = reader.IsDBNull(5) ? (byte?)null : reader.GetByte(5),
                            Telephone = reader.IsDBNull(6) ? null : reader.GetString(6),
                            ApplicationDate = reader.GetDateTime(7),
                            IsApproved = reader.GetBoolean(8)
                        });
                    }
                }
            }

            return applications;
        }

        public RegistrationApplication GetById(int applicationID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"
                    SELECT ApplicationID, FullName, PasswordHash, PermissionID, ClassID, Age, Telephone, ApplicationDate, IsApproved
                    FROM RegistrationApplications
                    WHERE ApplicationID = @ApplicationID", connection);

                command.Parameters.AddWithValue("@ApplicationID", applicationID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new RegistrationApplication
                        {
                            ApplicationID = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            PasswordHash = reader.GetString(2),
                            PermissionID = reader.GetInt32(3),
                            ClassID = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                            Age = reader.IsDBNull(5) ? (byte?)null : reader.GetByte(5),
                            Telephone = reader.IsDBNull(6) ? null : reader.GetString(6),
                            ApplicationDate = reader.GetDateTime(7),
                            IsApproved = reader.GetBoolean(8)
                        };
                    }
                }
            }

            return null;
        }

        public void Delete(int applicationID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"
                    DELETE FROM RegistrationApplications
                    WHERE ApplicationID = @ApplicationID", connection);

                command.Parameters.AddWithValue("@ApplicationID", applicationID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
