using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Euphoric.Logging.Memory
{
    public class LogEntryTest
    {
        [Fact]
        public void Customized_ToString()
        {
            var logEntry1 = new LogEntry(LogLevel.Information, "log message", "logger.category", null, new Dictionary<string, object>());
            Assert.Equal("[category;Information;log message]", logEntry1.ToString());
            
            var logEntry2 = new LogEntry(LogLevel.Warning, "long logged message, longest message, longest message, longest message, longest message", "non-dot-category", null, new Dictionary<string, object>());
            Assert.Equal("[non-dot-category;Warning;long logged message, longest message, longest mess...]", logEntry2.ToString());
            
            var logEntry3 = new LogEntry(LogLevel.Error, "error log", "logger.category", new Exception("Logged exception"), new Dictionary<string, object>());
            Assert.Equal("[category;Error;error log;Logged exception]", logEntry3.ToString());
        }
    }
}