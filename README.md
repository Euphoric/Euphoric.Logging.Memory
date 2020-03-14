# Euphoric.Logging.Memory

.NET Core implmenetation of `Microsoft.Extensions.Logging`, which logs into memory.
Useful for test automation.

[![Build Status](https://dev.azure.com/radekfalhar0821/Euphoric.Logging.Memory/_apis/build/status/Euphoric.Euphoric.Logging.Memory?branchName=master)](https://dev.azure.com/radekfalhar0821/Euphoric.Logging.Memory/_build/latest?definitionId=1&branchName=master)

# Usage

Install NuGet package : `Install-Package Euphoric.Logging.Memory`

Setup logger provider : `services.AddLogging(loggingBuilder => loggingBuilder.AddMemoryLogger());`

To retrieve the logs from memory : 

```
var memoryLogger = sp.GetRequiredService<IMemoryLogSource>();
var loggedEntries = memoryLogger.Logs;
```