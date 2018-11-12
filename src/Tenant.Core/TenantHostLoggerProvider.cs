using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Tenant.Core
{
    public class TenantHostLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, ILogger> _loggers = new ConcurrentDictionary<string, ILogger>();
        private static ILogger _logger;
        public TenantHostLoggerProvider(ILogger logger)
        {
            _logger = logger;
        }
        public TenantHostLoggerProvider()
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