using System;
using System.Collections.Generic;
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
        public LogEntry(LogLevel level, string message, string categoryName, Exception? exception, IReadOnlyDictionary<string, object> properties)
        {
            Level = level;
            Message = message;
            CategoryName = categoryName;
            Exception = exception;
            Properties = properties;
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

        /// <summary>
        /// Properties retrieved from state and scope.
        /// </summary>
        public IReadOnlyDictionary<string, object> Properties { get; }
    }
}