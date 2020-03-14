using System;
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
        public LogEntry(LogLevel level, string message, string categoryName, Exception? exception)
        {
            Level = level;
            Message = message;
            CategoryName = categoryName;
            Exception = exception;
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

        /// <summary>
        /// Logged exception
        /// </summary>
        public Exception? Exception { get; }
    }
}