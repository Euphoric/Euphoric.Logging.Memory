using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Euphoric.Logging.Memory
{
    /// <summary>
    /// Provider for logging into memory.
    /// </summary>
    public class MemoryLoggerProvider : ILoggerProvider, IMemoryLogSource, ISupportExternalScope
    {
        private readonly List<LogEntry> _logs = new List<LogEntry>();
        private IExternalScopeProvider? _scopeProvider;

        /// <inheritdoc />
        public IReadOnlyList<LogEntry> Logs => _logs.ToArray();

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return new MemoryLogger(this, categoryName, _scopeProvider);
        }

        /// <summary>
        /// Writes log entry into <see cref="Logs"/> collection.
        /// </summary>
        internal void LogEntry(LogEntry logEntry)
        {
            _logs.Add(logEntry);
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
