using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Euphoric.Logging.Memory
{
    /// <summary>
    /// Extension for <see cref="ILoggingBuilder"/>
    /// </summary>
    public static class LoggingBuilderExtensions
    {
        /// <summary>
        /// Adds memory logging provider and also adds <see cref="MemoryLoggerProvider"/> as singleton service.
        /// </summary>
        public static ILoggingBuilder AddMemoryLogger(this ILoggingBuilder logConfig)
        {
            var provider = new MemoryLoggerProvider();
            logConfig.Services.AddSingleton(provider);
            return logConfig.AddProvider(provider);
        }
    }
}
