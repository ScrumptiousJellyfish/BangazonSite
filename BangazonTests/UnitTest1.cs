using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BangazonTest
{
    public class ExerciseTest : IClassFixture<WebApplicationFactory<Bangazon.Startup>>
    {
        private readonly WebApplicationFactory<Bangazon.Startup> _factory;

        public ExerciseTest(WebApplicationFactory<Bangazon.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home")]
        [InlineData("/Home/About")]
        [InlineData("/Home/Privacy")]
        [InlineData("/Home/Contact")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}