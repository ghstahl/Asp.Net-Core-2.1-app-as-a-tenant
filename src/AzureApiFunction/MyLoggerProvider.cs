using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace AzureApiFunction
{
    public class MyLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, ILogger> _loggers = new ConcurrentDictionary<string, ILogger>();
        private static ILogger _logger;
        public MyLoggerProvider(ILogger logger)
        {
            _logger = logger;
        }
        public MyLoggerProvider()
        {
        }
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => _logger);
        }
        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}