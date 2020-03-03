using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Euphoric.Logging.Memory.Sample
{
    static class Program
    {
        static void Main(string[] args)
        {
            // register memory logger
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(logConfig =>
                    logConfig
                        .SetMinimumLevel(LogLevel.Trace)
                        .AddMemoryLogger());
            var sp = serviceCollection.BuildServiceProvider();

            // log messages
            var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("TestProgram");
            logger.LogInformation("Information message {number}", 13);
            logger.LogDebug("Debug message {text}", "PATH");
            logger.LogError(new Exception("Exception text."), "Error message");

            // retrieve logged events from memory logger provider
            var memoryLogger = sp.GetRequiredService<IMemoryLogSource>();
            
            foreach (var logEntry in memoryLogger.Logs)
            {
                Console.WriteLine($"{logEntry.CategoryName}:{logEntry.Level}:{logEntry.Message}");
            }
        }
    }
}
