using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Euphoric.Logging.Memory
{
    public class MemoryLoggerProvider : ILoggerProvider
    {
        private class MemoryLogger : ILogger
        {
            private readonly MemoryLoggerProvider _provider;
            private readonly string _name;

            public MemoryLogger(MemoryLoggerProvider provider, string name)
            {
                _provider = provider;
                _name = name;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                var message = formatter(state, exception);
                _provider._logs.Add(new LogEntry(logLevel, message, _name));
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                throw new NotImplementedException();
            }
        }

        private readonly List<LogEntry> _logs = new List<LogEntry>();

        public IReadOnlyCollection<LogEntry> Logs => _logs;

        public ILogger CreateLogger(string categoryName)
        {
            return new MemoryLogger(this, categoryName);
        }

        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}
