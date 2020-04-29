using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Euphoric.Logging.Memory
{
    /// <summary>
    /// Provider for logging into memory.
    /// </summary>
    public class MemoryLoggerProvider : ILoggerProvider, IMemoryLogSource, ISupportExternalScope
    {
        private class MemoryLogger : ILogger
        {
            private readonly MemoryLoggerProvider _provider;
            private readonly string _name;
            private readonly IExternalScopeProvider _scopeProvider;

            public MemoryLogger(MemoryLoggerProvider provider, string name, IExternalScopeProvider? scopeProvider)
            {
                _provider = provider;
                _name = name;
                _scopeProvider = scopeProvider ?? new LoggerExternalScopeProvider();
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

                _scopeProvider.ForEachScope(AddScopeToProperties, scopeProperties);

                return scopeProperties;
            }

            private static void AddScopeToProperties(object scope, Dictionary<string, object> properties)
            {
                if (scope is IEnumerable<KeyValuePair<string, object>> kvpScope)
                {
                    foreach (var keyValuePair in kvpScope)
                    {
                        properties[keyValuePair.Key] = keyValuePair.Value;
                    }
                }
                else
                {
                    if (properties.TryGetValue("Scope", out object scopePropertyList))
                    {
                        (scopePropertyList as List<object>)?.Add(scope);
                    }
                    else
                    {
                        properties["Scope"] = new List<object> {scope};
                    }
                }
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state) => _scopeProvider.Push(state);
        }

        private readonly List<LogEntry> _logs = new List<LogEntry>();
        private IExternalScopeProvider? _scopeProvider;

        /// <inheritdoc />
        public IReadOnlyList<LogEntry> Logs => _logs;

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return new MemoryLogger(this, categoryName, _scopeProvider);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Nothing to dispose
        }

        /// <inheritdoc />
        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }
    }
}
