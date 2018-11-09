using System;
using GraphQL.Execution;
using Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace AzureApiFunction
{
    public interface IServerRecord
    {
        TestServer TestServer { get; }
        string ServerName { get; }
    }
    public class ServerRecord<TStartup>: IServerRecord where TStartup: class
    {
        private ILogger _logger;
        public string ServerName { get; }
        private string _functionAppDirectory;
        public ServerRecord(string functionAppDirectory,string serverName,ILogger logger)
        {
            _functionAppDirectory = functionAppDirectory;
            ServerName = serverName;
            _logger = logger;
        }
       
        private TestServer _testServer;

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
                            logging.AddProvider(new MyLoggerProvider(_logger));
                        })
                        .UseContentRoot($"{_functionAppDirectory}/{ServerName}")
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