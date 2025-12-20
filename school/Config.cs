using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace school
{
    internal class AppSettings
    {
        public string MasterConnectionString { get; set; } = "Server=(localdb)\\MSSQLLocalDB;Database=master;Integrated Security=true;";
        public string ConnectionString { get; set; } = Form1.CONNECTION_STRING;
        public string LogPath { get; set; } = "logs";
        public string DatabaseName { get; set; } = "SchoolSystemTest";

        /// <summary>
        /// Загрузка настроек из appsettings.json (System.Text.Json - встроенный)
        /// </summary>
        public static AppSettings Load(string configPath = "appsettings.json")
        {
            var settings = new AppSettings();

            if (!File.Exists(configPath))
            {
                Console.WriteLine($"⚠️ Файл {configPath} не найден. Используются значения по умолчанию.");
                Directory.CreateDirectory(settings.LogPath);
                return settings;
            }

            try
            {
                string json = File.ReadAllText(configPath);
                settings = JsonSerializer.Deserialize<AppSettings>(json) ?? settings;

                Directory.CreateDirectory(settings.LogPath);

                FileLogger.logger.Info("AppSettings Load: " + configPath);
                FileLogger.logger.Info("AppSettings Load: " + json + " " + configPath);

                Console.WriteLine($"✅ Настройки загружены:");
                Console.WriteLine($"   DB: {settings.DatabaseName}");
                Console.WriteLine($"   Connection: {settings.ConnectionString.Substring(0, Math.Min(30, settings.ConnectionString.Length))}...");
                Console.WriteLine($"   Master: {settings.MasterConnectionString.Substring(0, Math.Min(30, settings.MasterConnectionString.Length))}...");
                Console.WriteLine($"   Logs: {settings.LogPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка загрузки настроек: {ex.Message}");
                Directory.CreateDirectory(settings.LogPath);
            }

            return settings;
        }
    }
}
