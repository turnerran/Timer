using System;
using WebApi.Services;
using Xunit;

namespace Tests
{
    public class DateTimeServiceTests
    {
        public DateTimeServiceTests()
        {
        }

        [Fact]
        public async void IsConvertInputToSchedueledTimeConvertingSecondsCorrect()
        {
            var dateTimeMock = new DateTimeService();
            var now = DateTime.UtcNow;
            var epochNow = now - new DateTime(1970, 1, 1);
            var time = dateTimeMock.ConvertInputToSchedueledTime(0, 0, 1);
            var epochSchedueled = time - new DateTime(1970, 1, 1);


            Assert.True((epochSchedueled - epochNow).Seconds == 1);
        }

        [Fact]
        public async void IsConvertInputToSchedueledTimeConvertingMinutesCorrect()
        {
            var dateTimeMock = new DateTimeService();
            var now = DateTime.UtcNow;
            var epochNow = now - new DateTime(1970, 1, 1);
            var time = dateTimeMock.ConvertInputToSchedueledTime(0, 1, 0);
            var epochSchedueled = time - new DateTime(1970, 1, 1);


            Assert.True((int)(epochSchedueled - epochNow).TotalSeconds == 60);
        }

        [Fact]
        public async void IsConvertInputToSchedueledTimeConvertingHoursCorrect()
        {
            var dateTimeMock = new DateTimeService();
            var now = DateTime.UtcNow;
            var epochNow = now - new DateTime(1970, 1, 1);
            var time = dateTimeMock.ConvertInputToSchedueledTime(1, 0, 0);
            var epochSchedueled = time - new DateTime(1970, 1, 1);


            Assert.True((int)(epochSchedueled - epochNow).TotalSeconds == 3600);
        }
    }
}