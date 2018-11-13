using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace XUnitTenantHost
{
    public class BadStaticControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public BadStaticControllerTests(TestServerFixture fixture)
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
        public async Task GetSummaryAsync()
        {
            var response = await _fixture.Client.GetAsync("tenant/one");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.ShouldNotBeNull();
        }
        [Fact]
        public async Task PostBadStaticAsync()
        {
            var value = "walk the dog";
            var response = await _fixture.Client.PostAsJsonAsync("tenant/one/api/badstatic", value);
            response.EnsureSuccessStatusCode();
            response = await _fixture.Client.GetAsync("tenant/one/api/badstatic");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.ShouldNotBeNull();
            var result = JsonConvert.DeserializeObject<List<string>>(responseString);
            result.Count.ShouldBe(1);
            result[0].ShouldBe(value);

            // So to pass the test proves that we have a problem and the badstatic implementation is here to highlight that statics 
            // pollute across tentants
            response = await _fixture.Client.GetAsync("tenant/two/api/badstatic");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            responseString.ShouldNotBeNull();
            result = JsonConvert.DeserializeObject<List<string>>(responseString);
            result.Count.ShouldBe(1);
            result[0].ShouldBe(value);
        }
      
    }
}
