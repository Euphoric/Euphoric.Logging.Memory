using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Euphoric.Logging.Memory
{
    public class LoggingBuilderExtensionsTest
    {
        [Fact]
        public void Adds_memory_logger_and_retrieves_it_from_services()
        {
            // register memory logger
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(logConfig =>
                logConfig.AddMemoryLogger());
            var sp = serviceCollection.BuildServiceProvider();

            // log message
            var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("TestProgram");
            logger.LogInformation("Information message {number}", 13);

            // retrieve logged events from memory logger provider
            var memoryLogger = sp.GetRequiredService<MemoryLoggerProvider>();
            
            var logEntry = Assert.Single(memoryLogger.Logs);
            Assert.NotNull(logEntry);

            Assert.Equal("TestProgram", logEntry.CategoryName);
            Assert.Equal(LogLevel.Information, logEntry.Level);
            Assert.Equal("Information message 13", logEntry.Message);
        }

        [Fact]
        public void Extension_methods_returns_same_loging_builder()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(logConfig =>
            {
                var res = logConfig.AddMemoryLogger();
                Assert.Equal(logConfig, res);
            });
        }
    }
}
