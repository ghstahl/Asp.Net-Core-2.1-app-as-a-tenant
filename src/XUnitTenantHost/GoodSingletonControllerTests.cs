using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace XUnitTenantHost
{
    public class GoodSingletonControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public GoodSingletonControllerTests(TestServerFixture fixture)
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
        public async Task PostGoodSingletonAsync()
        {
            var response = await _fixture.Client.GetAsync("tenant/one/api/goodsingleton/clear");
            response.EnsureSuccessStatusCode();
            response = await _fixture.Client.GetAsync("tenant/two/api/goodsingleton/clear");
            response.EnsureSuccessStatusCode();

            var value = "walk the dog";
            response = await _fixture.Client.PostAsJsonAsync("tenant/one/api/goodsingleton", value);
            response.EnsureSuccessStatusCode();
            response = await _fixture.Client.GetAsync("tenant/one/api/goodsingleton");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.ShouldNotBeNull();
            var result = JsonConvert.DeserializeObject<List<string>>(responseString);
            result.Count.ShouldBe(1);
            result[0].ShouldBe(value);


            response = await _fixture.Client.GetAsync("tenant/two/api/goodsingleton");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            responseString.ShouldNotBeNull();
            result = JsonConvert.DeserializeObject<List<string>>(responseString);
            result.Count.ShouldBe(0);

        }
        [Fact]
        public async Task AssureGoodSingletonBoundariesAsync()
        {
            var response = await _fixture.Client.GetAsync("tenant/one/api/goodsingleton/clear");
            response.EnsureSuccessStatusCode();
            response = await _fixture.Client.GetAsync("tenant/two/api/goodsingleton/clear");
            response.EnsureSuccessStatusCode();
            response = await _fixture.Client.GetAsync("tenant/one/api/goodsingleton2/clear");
            response.EnsureSuccessStatusCode();
            response = await _fixture.Client.GetAsync("tenant/two/api/goodsingleton2/clear");
            response.EnsureSuccessStatusCode();

            var value = "walk the dog";
            response = await _fixture.Client.PostAsJsonAsync("tenant/one/api/goodsingleton", value);
            response.EnsureSuccessStatusCode();
            response = await _fixture.Client.GetAsync("tenant/one/api/goodsingleton");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.ShouldNotBeNull();
            var result = JsonConvert.DeserializeObject<List<string>>(responseString);
            result.Count.ShouldBe(1);
            result[0].ShouldBe(value);


            response = await _fixture.Client.GetAsync("tenant/two/api/goodsingleton2");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            responseString.ShouldNotBeNull();
            result = JsonConvert.DeserializeObject<List<string>>(responseString);
            result.Count.ShouldBe(0);

            value = "Feed The Cat";
            response = await _fixture.Client.PostAsJsonAsync("tenant/one/api/goodsingleton2", value);
            response.EnsureSuccessStatusCode();
            response = await _fixture.Client.GetAsync("tenant/one/api/goodsingleton2");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            responseString.ShouldNotBeNull();
            result = JsonConvert.DeserializeObject<List<string>>(responseString);
            result.Count.ShouldBe(1);
            result[0].ShouldBe(value);

        }
    }
}