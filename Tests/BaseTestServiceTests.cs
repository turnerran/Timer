using AutoFixture;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    public class BaseTestService
    {
        internal Mock<IHttpClientFactory> _httpClientFactory;
        public BaseTestService()
        {
            SetHttpClientMock();
        }

        private void SetHttpClientMock()
        {
            _httpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var fixture = new Fixture();

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(fixture.Create<String>()),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            client.BaseAddress = fixture.Create<Uri>();
            _httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
        }
    }
}
