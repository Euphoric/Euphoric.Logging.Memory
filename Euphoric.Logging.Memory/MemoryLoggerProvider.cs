using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            private readonly IExternalScopeProvider _scopeProvider = new LoggerExternalScopeProvider();

            public MemoryLogger(MemoryLoggerProvider provider, string name)
            {
                _provider = provider;
                _name = name;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                var message = formatter(state, exception);
                var properties = GetScopeInformation();
                if (state is IEnumerable<KeyValuePair<string, object>> kvpState)
                {
                    foreach (var keyValuePair in kvpState)
                    {
                        properties[keyValuePair.Key] = keyValuePair.Value;
                    }
                }
                _provider._logs.Add(new LogEntry(logLevel, message, _name, exception, properties));
            }

            private Dictionary<string, object> GetScopeInformation()
            {
                var scopeProperties = new Dictionary<string, object>();

                _scopeProvider.ForEachScope((scope, properites) =>
                    {
                        if (properites.TryGetValue("Scope", out object scopePropertyList))
                        {
                            (scopePropertyList as List<object>)?.Add(scope);
                        }
                        else
                        {
                            properites["Scope"] = new List<object> {scope};
                        }

                    }, scopeProperties);

                return scopeProperties;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state) => _scopeProvider.Push(state);
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
