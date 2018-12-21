using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Kts.RefactorThis.Api.Integration.Tests
{
    [TestFixture]
    public class HealthTests
    {
        private TestWebApplicationFactory _sut;
        private HttpClient Client;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _sut = new TestWebApplicationFactory("health");
            Client = _sut.CreateClient();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _sut.Dispose();
        }

        // Tests real implementation and configuration
        [Test]
        public async Task Given_ApiIsRunningAndWellConfigured_WhenPingIsInvoked_ReturnsOkResult()
        {
            using (var client = _sut.CreateClient())
            {
                var response = await _sut.CreateClient().GetAsync("/health/ping");

                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }               
        }  
    }
}
