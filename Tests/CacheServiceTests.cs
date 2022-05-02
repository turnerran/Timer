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