using System.IO;

namespace calc.src.main.services
{
    public class HistoryService
    {
        private readonly string _logFile = "history.log";

        public void SaveRecord(string expression, string result)
        {
            string logLine = $"{expression} = {result}\r\n";
            File.AppendAllText(_logFile, logLine);
        }

        public string GetHistory()
        {
            if (File.Exists(_logFile))
            {
                return File.ReadAllText(_logFile);
            }
            return string.Empty;
        }

        public void ClearHistory()
        {
            File.WriteAllText(_logFile, string.Empty);
        }
    }
}