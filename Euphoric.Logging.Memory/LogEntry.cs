using Microsoft.Extensions.Logging;

namespace Euphoric.Logging.Memory
{
    public class LogEntry
    {
        public LogEntry(LogLevel level, string message, string categoryName)
        {
            Level = level;
            Message = message;
            CategoryName = categoryName;
        }

        public string CategoryName { get; }
        public LogLevel Level { get; }
        public string Message { get; }
    }
}