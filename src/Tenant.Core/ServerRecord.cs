using Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tenant.Core.Host;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Tenant.Core
{
    public class ServerRecord<TStartup>: IServerRecord where TStartup: class
    {
        private ILogger _logger;
        public string BaseUrl { get; set; }
        public PathString PathStringBaseUrl { get; set; }
        private string _functionAppDirectory;
        public ServerRecord(string functionAppDirectory,string settingsPath, ILogger logger)
        {
            _functionAppDirectory = functionAppDirectory;
            _settingsPath = settingsPath;
            _logger = logger;
        }
       
        private TestServer _testServer;
        private string _settingsPath;

        public TestServer TestServer
        {
            get
            {
                if (_testServer == null)
                {
                    var webHostBuilder = new WebHostBuilder()
                        .ConfigureLogging((hostingContext, logging) =>
                        {
                            logging.ClearProviders();
                            logging.AddProvider(new TenantHostLoggerProvider(_logger));
                        })
                        .UseContentRoot($"{_functionAppDirectory}/{_settingsPath}")
                        .UseStartup<TStartup>()
                        .ConfigureServices(s =>
                        s.AddSingleton<IStartupConfigurationService, NullStartupConfigurationService>());
                    var server = new TestServer(webHostBuilder);
                    _testServer = server;
                }
                return _testServer;
            }
        }
    }
}