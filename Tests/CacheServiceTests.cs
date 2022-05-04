using Moq;
using System;
using WebApi.Services;
using WebApplication1.Models.Domains;
using Xunit;

namespace Tests
{
    public class CacheServiceTests
    {
        public CacheServiceTests()
        {
        }

        [Fact]
        public async void IsFetchingTaskFromCacheReturnTrue()
        {
            var _dateTimeService = new Mock<IDateTimeService>();
            var cacheService = new CacheService();
            var url = "http://www.blabla.com";
            var id = 1;
            var task = new SchedueledTask
            {
                Id = id,
                IsCompleted = false,
                FireEventTime = _dateTimeService.Object.ConvertInputToSchedueledTime(0, 0, 0),
                Url = url
            };
            cacheService.Add(task);

            var retrievedTask = cacheService.GetTaskById(task.Id);
            Assert.True(url == retrievedTask.Url);
            Assert.True(id == retrievedTask.Id);
        }
    }
}