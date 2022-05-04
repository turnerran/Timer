using WebApi.Services;
using WebApplication1.Models.Domains;

namespace WebApplication1.Services
{
    public interface ITimedHostedService
    {
        Task SetTimer(SchedueledTask task);
    }
    public class TimedHostedService : ITimedHostedService
    {
        private ITaskActionService _taskActionService;
        private IServiceScopeFactory _serviceScopeFactory;
        private ICacheService _cacheService;
        public TimedHostedService(ITaskActionService taskActionService,
                                    IServiceScopeFactory scopeFactory, 
                                    ICacheService cacheService)
        {
            _taskActionService = taskActionService;
            _serviceScopeFactory = scopeFactory;
            _cacheService = cacheService;
        }

        public Task SetTimer(SchedueledTask task)
        {
            var aTimer = new System.Timers.Timer();
            aTimer.Elapsed += async delegate { await OnTimedEvent(task); };
            aTimer.Interval = (int)(task.FireEventTime - DateTime.UtcNow).TotalMilliseconds;
            aTimer.Enabled = true;
            aTimer.AutoReset = false;

            return Task.CompletedTask;
        }
        private async Task OnTimedEvent(SchedueledTask task)
        {
            var isCompleted = await _taskActionService.DoAction(task);
            if (isCompleted)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedService = scope.ServiceProvider.GetRequiredService<ISchedueledTaskService>();
                    await scopedService.MarkTaskAsCompleted(task.Id);
                }
            }

            _cacheService.Remove(task.Id);

        }
    }
}
