using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace XUnitTenantHost
{
    public class FormControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public FormControllerTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public void AssureFixture()
        {
            _fixture.ShouldNotBeNull();
            var client = _fixture.Client;
            client.ShouldNotBeNull();
        }

        [Fact]
        public async Task PostFormAsync()
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("comment", "What is up?"),
                new KeyValuePair<string, string>("questionId", Guid.NewGuid().ToString())
            });

            var response = await _fixture.Client.PostAsJsonAsync("tenant/one/api/FORM", formContent);
            response.EnsureSuccessStatusCode();
          
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.ShouldNotBeNull();

        }
      
    }
}
