using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;

namespace Euphoric.Logging.Memory
{
    /// <summary>
    /// Provider for logging into memory.
    /// </summary>
    public class MemoryLoggerProvider : ILoggerProvider, IMemoryLogSource
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
                return NullScope.Instance;
            }
        }

        private readonly List<LogEntry> _logs = new List<LogEntry>();

        /// <inheritdoc />
        public IReadOnlyList<LogEntry> Logs => _logs;

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return new MemoryLogger(this, categoryName);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}
