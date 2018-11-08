using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace AzureApiFunction
{
    public class ServerRecord
    {
        public string Server { get; set; }
        public IWebHostBuilder WebHostBuilder { get; set; }
        public string BaseAddress { get; set; }
        private TestServer _testServer;
        public TestServer TestServer
        {
            get
            {
                if (_testServer == null)
                {
                    var server = new TestServer(WebHostBuilder) {BaseAddress = new Uri(BaseAddress)};
                    _testServer = server;
                }

                return _testServer;
            }
        
        }
    }
}