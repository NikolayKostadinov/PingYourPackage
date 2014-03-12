namespace PingYourPackage.API.Test.MessageHandlers
{
    using PingYourPackage.API.MessageHandlers;
    using PingYourPackage.API.Test.TestHelpers;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Xunit;

    public class RequireHttpsMessageHandlerTest
    {
        [Fact]
        public async Task ReturnsForbiddenIfRequestIsNotOverHttps()
        {
            // Arange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080");
            var requireHtttpsMessageHandler = new RequireHttpsMessageHandler();

            // Act
            var response = await requireHtttpsMessageHandler.InvokeAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task ReturnsDelegatedStatusCodeWhenRequestIsOverHttps()
        {
            // Arange
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:8080");
            var requireHtttpsMessageHandler = new RequireHttpsMessageHandler();

            // Act
            var response = await requireHtttpsMessageHandler.InvokeAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
