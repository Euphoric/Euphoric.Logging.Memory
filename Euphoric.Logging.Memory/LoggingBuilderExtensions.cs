using System;
using System.Collections.Generic;
using System.Text;
using Euphoric.Logging.Memory;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Logging
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
