using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using WebApi.Helpers;
using WebApi.Services;
using WebApplication1;
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

            var cacheService = new CacheService();
            var url = "http://www.blabla.com";
            var id = 1;
            var task = new SchedueledTask
            {
                Id = id,
                IsCompleted = false,
                FireEventTime = DateTime.UtcNow,
                Url = url
            };
            cacheService.Add(task);

            var retrievedTask = cacheService.GetTaskById(task.Id);
            Assert.True(url == retrievedTask.Url);
            Assert.True(id == retrievedTask.Id);
        }
    }
}