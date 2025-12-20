using Microsoft.Data.SqlClient;
using school.Controllers;
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

        private List<EventChange> pendingChanges = new List<EventChange>();

        /// <summary>
        /// Модель изменения события
        /// </summary>
        public class EventChange
        {
            public string Action { get; set; }
            public Event Event { get; set; } = new Event();
            public int OriginalEventID { get; set; } = 0; 
        }

        /// <summary>
        /// Добавляет изменение события в очередь
        /// </summary>
        public void AddEventChange(string action, Event eventModel)
        {
            pendingChanges.Add(new EventChange
            {
                Action = action.ToUpper(),
                Event = eventModel
            });

            FileLogger.logger.Info($"EventsController.AddEventChange Добавлено изменение события: {action} (ID: {eventModel.EventID}, Название: {eventModel.EventName})");
        }

        /// <summary>
        /// Выполняет все изменения событий и очищает очередь
        /// </summary>
        public int CommitEventChanges()
        {
            if (pendingChanges.Count == 0) return 0;

            int processed = 0;
            try
            {
                using (var connection = new SqlConnection(Form1.CONNECTION_STRING))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (var change in pendingChanges)
                            {
                                switch (change.Action)
                                {
                                    case "EDIT":
                                    case "ADD":
                                        change.Event.EventID = UpsertEvent(change.Event);
                                        processed++;
                                        break;
                                    case "DELETE":
                                        DeleteEvent(change.Event.EventID);
                                        processed++;
                                        break;
                                }
                            }
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                pendingChanges.Clear();
                FileLogger.logger.Info($"EventsController.CommitEventChanges - Сохранено {processed} изменений событий");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"EventsController.CommitEventChanges - Ошибка сохранения событий: {ex.Message}");
                throw;
            }
            return processed;
        }

        /// <summary>
        /// Удаляет событие по ID
        /// Возвращает true если удалено успешно, false если не найдено
        /// </summary>
        public bool DeleteEvent(int eventId)
        {
            if (eventId < 1) return false;

            try
            {
                using (var connection = new SqlConnection(Form1.CONNECTION_STRING))
                {
                    connection.Open();

                    var deleteQuery = "DELETE FROM Events WHERE EventID = @EventID";
                    using (var deleteCmd = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCmd.Parameters.AddWithValue("@EventID", eventId);
                        int rowsAffected = deleteCmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"EventsController.DeleteEvent - Ошибка удаления события ID {eventId}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Удаляет событие по модели (совместимо с очередью изменений)
        /// </summary>
        public bool DeleteEvent(Event eventModel)
        {
            return DeleteEvent(eventModel.EventID);
        }

        /// <summary>
        /// Вставляет или обновляет мероприятие по логике UPSERT
        /// Если EventID >= 0 и существует - обновляет
        /// Если EventID < 0 или не существует - вставляет новое с автоинкрементом
        /// </summary>
        public int UpsertEvent(Event eventModel)
        {
            using (var connection = new SqlConnection(Form1.CONNECTION_STRING))
            {
                connection.Open();

                bool exists = false;
                if (eventModel.EventID >= 0)
                {
                    var existsQuery = "SELECT COUNT(*) FROM Events WHERE EventID = @EventID";
                    using (var checkCmd = new SqlCommand(existsQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@EventID", eventModel.EventID);
                        exists = (int)checkCmd.ExecuteScalar() > 0;
                    }
                }

                if (exists)
                {
                    var updateQuery = @"
                UPDATE Events 
                SET EventName = @EventName, EventTime = @EventTime, Location = @Location 
                WHERE EventID = @EventID";

                    using (var updateCmd = new SqlCommand(updateQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@EventName", eventModel.EventName);
                        updateCmd.Parameters.AddWithValue("@EventTime", eventModel.EventTime);
                        updateCmd.Parameters.AddWithValue("@Location", eventModel.Location);
                        updateCmd.Parameters.AddWithValue("@EventID", eventModel.EventID);
                        updateCmd.ExecuteNonQuery();
                        return eventModel.EventID;
                    }
                }
                else
                {
                    var insertQuery = @"
                INSERT INTO Events (EventName, EventTime, Location)
                OUTPUT INSERTED.EventID
                VALUES (@EventName, @EventTime, @Location)";

                    using (var insertCmd = new SqlCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@EventName", eventModel.EventName);
                        insertCmd.Parameters.AddWithValue("@EventTime", eventModel.EventTime);
                        insertCmd.Parameters.AddWithValue("@Location", eventModel.Location);

                        int newId = (int)insertCmd.ExecuteScalar();
                        eventModel.EventID = newId;
                        return newId;
                    }
                }
            }
        }

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

                FileLogger.logger.Info($"EventsController.GetAllEvents: возвращено {events.Count} мероприятий");
                return events;
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"EventsController.GetAllEvents: {ex.Message}");
                return new List<Event>();
            }
        }
    }
}
