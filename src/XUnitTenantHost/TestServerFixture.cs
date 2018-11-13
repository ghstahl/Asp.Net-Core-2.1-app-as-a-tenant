using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.PlatformAbstractions;
using TenantHost;

namespace XUnitTenantHost
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServer;
        public HttpClient Client { get; }

        public TestServerFixture()
        {
            var contentRootPath = GetContentRootPath();
            var builder = new WebHostBuilder()
                .UseContentRoot(contentRootPath)
                .UseEnvironment("Development")
                .ConfigureServices(services =>
                {
                    services.TryAddTransient<IDefaultHttpClientFactory, TestDefaultHttpClientFactory>();
                })
                .UseStartup<Startup>();  // Uses Start up class from your API Host project to configure the test server

            _testServer = new TestServer(builder);
            Client = _testServer.CreateClient();

        }

        private string GetContentRootPath()
        {
            var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            var relativePathToHostProject = @"..\..\..\..\TenantHost";
            return Path.Combine(testProjectPath, relativePathToHostProject);
        }

        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
        }
    }
}