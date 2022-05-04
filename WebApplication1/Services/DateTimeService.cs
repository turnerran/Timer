namespace WebApi.Services
{
    public interface IDateTimeService
    {
        DateTime ConvertInputToSchedueledTime(int hours, int minutes,
                                                     int seconds);
    }

    public class DateTimeService : IDateTimeService
    {
        public DateTime ConvertInputToSchedueledTime(int hours, int minutes,
                                                     int seconds)
        {
            var scheduledTime = DateTime.UtcNow;
            scheduledTime = scheduledTime.AddHours(hours);
            scheduledTime = scheduledTime.AddMinutes(minutes);
            scheduledTime = scheduledTime.AddSeconds(seconds);

            return scheduledTime;
        }
    }
}