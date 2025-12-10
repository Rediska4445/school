using System;
using System.IO;
using System.Linq;

namespace school
{
    public class FileLogger : IDisposable
    {
        public static FileLogger logger = new FileLogger();

        private readonly string _logFilePath;
        private readonly object _lockObject = new object();
        private bool _disposed = false;

        /// <summary>
        /// Создаёт логгер с выводом в файл в папке приложения
        /// </summary>
        /// <param name="logFileName">Имя файла лога (например, "app.log")</param>
        public FileLogger(string logFileName = "app.log")
        {
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            _logFilePath = Path.Combine(appDir, logFileName);

            // Создаём директорию, если её нет
            Directory.CreateDirectory(appDir);

            Log("INFO", $"Логгер запущен. Файл: {_logFilePath}");
        }

        /// <summary>
        /// Записывает сообщение в лог
        /// </summary>
        public void Log(string level, string message)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}";

            lock (_lockObject)
            {
                try
                {
                    File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    // Тихо игнорируем ошибки записи
                    Console.WriteLine($"Ошибка записи в лог: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// INFO уровень
        /// </summary>
        public void Info(string message) => Log("INFO", message);

        /// <summary>
        /// WARNING уровень
        /// </summary>
        public void Warn(string message) => Log("WARN", message);

        /// <summary>
        /// ERROR уровень
        /// </summary>
        public void Error(string message) => Log("ERROR", message);

        /// <summary>
        /// ERROR + Exception
        /// </summary>
        public void Error(string message, Exception ex)
        {
            Log("ERROR", $"{message}. Исключение: {ex}");
        }

        /// <summary>
        /// DEBUG уровень
        /// </summary>
        public void Debug(string message) => Log("DEBUG", message);

        /// <summary>
        /// Читает последние N строк из лога
        /// </summary>
        public string[] ReadLastLines(int count = 50)
        {
            lock (_lockObject)
            {
                try
                {
                    if (!File.Exists(_logFilePath)) return new string[0];

                    string[] allLines = File.ReadAllLines(_logFilePath);
                    int startIndex = Math.Max(0, allLines.Length - count);
                    return allLines.Skip(startIndex).ToArray();
                }
                catch
                {
                    return new string[0];
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Log("INFO", "Логгер остановлен");
                }
                _disposed = true;
            }
        }
    }
}
