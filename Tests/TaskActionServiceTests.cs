using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using WebApi.Services;
using WebApplication1;
using WebApplication1.Models.Domains;
using Xunit;

namespace Tests
{
    public class TaskActionServiceTests : BaseTestService
    {
        [Fact]
        public async void IsDoActionWithNegativeNumberReturnFalse()
        {
            var loggerMock = new Mock<ILogger<TaskActionService>>();
            var taskActionService = new TaskActionService(_httpClientFactory.Object,
                                                              loggerMock.Object);

            var task = new SchedueledTask
            {
                FireEventTime = DateTime.UtcNow.AddMinutes(1),
                Id = 0,
                IsCompleted = false,
                Url = "www.one.co.il"
            };

            var res = await taskActionService.DoAction(task);

            Assert.False(res);
        }

        [Fact]
        public async void IsDoActionWithInValidUrlReturnFalse()
        {
            var loggerMock = new Mock<ILogger<TaskActionService>>();
            var taskActionService = new TaskActionService(_httpClientFactory.Object,
                                                          loggerMock.Object);

            var task = new SchedueledTask
            {
                FireEventTime = DateTime.UtcNow.AddMinutes(1),
                Id = 1,
                IsCompleted = false,
                Url = "invalid-url"
            };

            var res = await taskActionService.DoAction(task);

            Assert.False(res);
        }

        [Fact]
        public async void IsDoActionWithEmptyUrlReturnFalse()
        {
            var loggerMock = new Mock<ILogger<TaskActionService>>();
            var taskActionService = new TaskActionService(_httpClientFactory.Object,
                                                          loggerMock.Object);

            var task = new SchedueledTask
            {
                FireEventTime = DateTime.UtcNow.AddMinutes(1),
                Id = 1,
                IsCompleted = false,
                Url = ""
            };

            var res = await taskActionService.DoAction(task);

            Assert.False(res);
        }

        [Fact]
        public async Task IsDoActionWithValidUrlReturnTrue()
        {
            var loggerMock = new Mock<ILogger<TaskActionService>>();
            var httpClientFactory = _httpClientFactory;

            var taskActionService = new TaskActionService(httpClientFactory.Object,
                                                              loggerMock.Object);

            var task = new SchedueledTask
            {
                FireEventTime = DateTime.UtcNow.AddMinutes(1),
                Id = 41655789,
                IsCompleted = false,
                Url = "https://gorest.co.in/public/v2/users"
            };

            var res = await taskActionService.DoAction(task);

            Assert.True(res);
        }
    }
}