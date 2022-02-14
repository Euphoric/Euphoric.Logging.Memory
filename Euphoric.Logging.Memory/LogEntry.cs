using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <inheritdoc />
        public override string ToString()
        {
            var lastCategoryPart = CategoryName.Split('.').Last();
            var trimmedLogMessage = Message.Substring(0, Math.Min(Message.Length, 50));
            if (trimmedLogMessage.Length == 50)
                trimmedLogMessage += "...";
            List<string> parts = new List<string>()
            {
                lastCategoryPart,
                Level.ToString(),
                trimmedLogMessage
            };
            if (Exception != null)
            {
                parts.Add(Exception.Message);
            }
            return $"[{string.Join(";", parts)}]";
        }
    }
}