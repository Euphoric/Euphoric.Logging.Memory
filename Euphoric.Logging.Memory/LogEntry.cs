using Microsoft.Extensions.Logging;

namespace Euphoric.Logging.Memory
{
    /// <summary>
    /// Logged entry
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Creates logged entry
        /// </summary>
        public LogEntry(LogLevel level, string message, string categoryName)
        {
            Level = level;
            Message = message;
            CategoryName = categoryName;
        }

        /// <summary>
        /// Name of a logger category.
        /// </summary>
        public string CategoryName { get; }

        /// <summary>
        /// Logged level
        /// </summary>
        public LogLevel Level { get; }

        /// <summary>
        /// Logged message
        /// </summary>
        public string Message { get; }
    }
}