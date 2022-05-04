using AutoFixture;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Services;
using WebApplication1.Controllers;
using WebApplication1.Models.Requests;
using WebApplication1.Services;
using Xunit;

namespace Tests
{
    public class TimerControllerTests : BaseTestService
    {
        public TimerControllerTests() : base()
        {
        }

        [Fact]
        public async void IsCallingGetWithIdZeroThrowsError()
        {
            var _schedueledTaskService = new Mock<ISchedueledTaskService>();
            var _dateTimeService = new Mock<IDateTimeService>();
            var _timedHostedService = new Mock<ITimedHostedService>();
            var _cacheService = new Mock<ICacheService>();
            var timersController = new TimersController(_schedueledTaskService.Object,
                                                        _dateTimeService.Object,
                                                        _timedHostedService.Object,
                                                        _cacheService.Object);

            var id = 0;

            await Assert.ThrowsAsync<ArgumentException>(async () => await timersController.Get(id));
        }

        [Fact]
        public async void IsCallingPostWithNegativeTimeThrowsError()
        {
            var _schedueledTaskService = new Mock<ISchedueledTaskService>();
            var _timedHostedService = new Mock<ITimedHostedService>();
            var _dateTimeService = new Mock<IDateTimeService>();
            var _cacheService = new Mock<ICacheService>();
            var timersController = new TimersController(_schedueledTaskService.Object,
                                                        _dateTimeService.Object,
                                                        _timedHostedService.Object,
                                                        _cacheService.Object);

            var urlTask = new UrlTask
            {
                Hours = 0,
                Minutes = 0,
                Seconds = -1,
                Url = "www.one.co.il"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await timersController.Post(urlTask));
        }

        [Fact]
        public async void IsCallingPostWithInValidUrlThrowsError()
        {
            var _schedueledTaskService = new Mock<ISchedueledTaskService>();
            var _timedHostedService = new Mock<ITimedHostedService>();
            var _cacheService = new Mock<ICacheService>();
            var _dateTimeService = new Mock<IDateTimeService>();
            var timersController = new TimersController(_schedueledTaskService.Object,
                                                        _dateTimeService.Object,
                                                        _timedHostedService.Object,
                                                        _cacheService.Object);

            var urlTask = new UrlTask
            {
                Hours = 0,
                Minutes = 0,
                Seconds = 1,
                Url = "www"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await timersController.Post(urlTask));
        }

        [Fact]
        public async void IsCallingPostWithEmptyUrlThrowsError()
        {
            var _schedueledTaskService = new Mock<ISchedueledTaskService>();
            var _timedHostedService = new Mock<ITimedHostedService>();
            var _cacheService = new Mock<ICacheService>();
            var _dateTimeService = new Mock<IDateTimeService>();
            var timersController = new TimersController(_schedueledTaskService.Object,
                                                        _dateTimeService.Object,
                                                        _timedHostedService.Object,
                                                        _cacheService.Object);

            var urlTask = new UrlTask
            {
                Hours = 0,
                Minutes = 0,
                Seconds = 1,
                Url = ""
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await timersController.Post(urlTask));
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