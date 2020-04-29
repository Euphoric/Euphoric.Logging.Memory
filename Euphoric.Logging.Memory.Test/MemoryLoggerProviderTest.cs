using System;
using System.Collections.Generic;
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

        [Fact]
        public void Doesnt_log_exception_when_not_present()
        {
            MemoryLoggerProvider provider = new MemoryLoggerProvider();
            var logger = provider.CreateLogger("TestLogger");
            
            logger.LogInformation("Message");

            var message = Assert.Single(provider.Logs);
            Assert.NotNull(message);
            Assert.Null(message.Exception);
        }

        [Fact]
        public void Logs_exception()
        {
            MemoryLoggerProvider provider = new MemoryLoggerProvider();
            var logger = provider.CreateLogger("TestLogger");

            var loggedException = new Exception("Exception test text");
            logger.LogError(loggedException, "Message");

            var message = Assert.Single(provider.Logs);
            Assert.NotNull(message);
            Assert.Equal(loggedException, message.Exception);
        }

        [Theory]
        [InlineData("string value")]
        [InlineData(1317)]
        [InlineData(1317.0d)]
        public void Logs_state_properties(object value)
        {
            MemoryLoggerProvider provider = new MemoryLoggerProvider();
            var logger = provider.CreateLogger("TestLogger");

            logger.LogInformation("Message {PropertyValue}", value);

            var log = Assert.Single(provider.Logs);

            Assert.Equal(value, log.Properties["PropertyValue"]);
        }

        [Fact]
        public void Logs_multiple_state_properties()
        {
            MemoryLoggerProvider provider = new MemoryLoggerProvider();
            var logger = provider.CreateLogger("TestLogger");

            var dateTime = new DateTime(2020, 04, 29, 11, 13, 21);

            logger.LogInformation("Message {Prop1} {Prop2} {Prop3}", true, 19, dateTime);

            var log = Assert.Single(provider.Logs);

            Assert.Equal(true, log.Properties["Prop1"]);
            Assert.Equal(19, log.Properties["Prop2"]);
            Assert.Equal(dateTime, log.Properties["Prop3"]);
        }

        [Fact]
        public void Logs_scope_property()
        {
            MemoryLoggerProvider provider = new MemoryLoggerProvider();
            var logger = provider.CreateLogger("TestLogger");

            using (logger.BeginScope("Scope value"))
            {
                logger.LogInformation("Log within scope");
            }
            
            var log = Assert.Single(provider.Logs);
            var scopeArray = Assert.IsAssignableFrom<IEnumerable<object>>(log.Properties["Scope"]);
            Assert.Equal(new object[]{"Scope value"}, scopeArray);
        }

        [Fact]
        public void Doesnt_log_scope_property_outside_scope()
        {
            MemoryLoggerProvider provider = new MemoryLoggerProvider();
            var logger = provider.CreateLogger("TestLogger");

            using (logger.BeginScope("Scope value"))
            {
            }
            
            logger.LogInformation("Log outside scope");

            var log = Assert.Single(provider.Logs);
            Assert.False(log.Properties.ContainsKey("Scope"));
        }

        [Fact]
        public void Logs_nested_scopes()
        {
            MemoryLoggerProvider provider = new MemoryLoggerProvider();
            var logger = provider.CreateLogger("TestLogger");

            using (logger.BeginScope("Value 1"))
            {
                using (logger.BeginScope(false))
                {
                    using (logger.BeginScope(3))
                    {
                        logger.LogInformation("Log within scope");
                    }
                }
            }
            
            var log = Assert.Single(provider.Logs);
            var scopeArray = Assert.IsAssignableFrom<IEnumerable<object>>(log.Properties["Scope"]);
            Assert.Equal(new object[] {"Value 1", false, 3}, scopeArray);
        }
    }
}
