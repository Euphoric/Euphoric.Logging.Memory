using System;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Euphoric.Logging.Memory
{
    public class MemoryLoggerProviderTest
    {
        [Fact]
        public void LoggerProvider_has_no_logs_after_creation()
        {
            MemoryLoggerProvider provider = new MemoryLoggerProvider();
            Assert.Empty(provider.Logs);
        }

        [Fact]
        public void Dispose_doesnt_fail()
        {
            MemoryLoggerProvider provider = new MemoryLoggerProvider();
            provider.Dispose();
        }

        [Fact]
        public void LoggerProvider_creates_Logger()
        {
            MemoryLoggerProvider provider = new MemoryLoggerProvider();
            var logger = provider.CreateLogger("TestLogger");
            Assert.NotNull(logger);
        }

        [Fact]
        public void Logger_is_always_enabled()
        {
            MemoryLoggerProvider provider = new MemoryLoggerProvider();
            var logger = provider.CreateLogger("TestLogger");
            Assert.True(logger.IsEnabled(LogLevel.Trace));
        }

        [Fact]
        public void Logger_scope_doesnt_throw()
        {
            MemoryLoggerProvider provider = new MemoryLoggerProvider();
            var logger = provider.CreateLogger("TestLogger");
            var scope = logger.BeginScope("Scope");
            Assert.NotNull(scope);
            scope.Dispose();
        }

        [Fact]
        public void Logger_logs_message()
        {
            MemoryLoggerProvider provider = new MemoryLoggerProvider();
            var logger = provider.CreateLogger("TestLogger");
            
            logger.LogInformation("Message");

            var message = Assert.Single(provider.Logs);
            Assert.NotNull(message);
            Assert.Equal("TestLogger", message.CategoryName);
            Assert.Equal(LogLevel.Information, message.Level);
            Assert.Equal("Message", message.Message);
        }
    }
}
