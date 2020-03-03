using System.Collections.Generic;

namespace Euphoric.Logging.Memory
{
    /// <summary>
    /// Source of logs entries stored in memory.
    /// </summary>
    public interface IMemoryLogSource
    {
        /// <summary>
        /// Collection of logged entries.
        /// </summary>
        IReadOnlyList<LogEntry> Logs { get; }
    }
}