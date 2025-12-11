using Microsoft.Data.SqlClient;
using school.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school
{
    public class EventsController
    {
        public static EventsController _controller = new EventsController();

        public List<Event> GetAllEvents()
        {
            var events = new List<Event>();

            try
            {
                string sql = @"
            SELECT EventID, EventName, EventTime, Location
            FROM Events
            ORDER BY EventTime DESC";

                using (var connection = new SqlConnection(Form1.CONNECTION_STRING))
                using (var command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events.Add(new Event
                            {
                                EventID = reader.GetInt32(0),
                                EventName = reader.GetString(1),
                                EventTime = reader.GetDateTime(2),
                                Location = reader.GetString(3)
                            });
                        }
                    }
                }

                FileLogger.logger.Info($"✅ GetAllEvents: возвращено {events.Count} мероприятий");
                return events;
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"❌ GetAllEvents: {ex.Message}");
                return new List<Event>();
            }
        }
    }
}
