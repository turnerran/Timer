using WebApplication1;
using WebApplication1.Models.Domains;

namespace WebApi.Services
{
    public interface IOnInitService
    {
        Task DoOverDueTasks();
    }

    public class OnInitService : IOnInitService
    {
        private ILogger<OnInitService> _logger;
        private ISchedueledTaskService _schedueledTaskService;
        private ITaskActionService _taskActionService;
        public OnInitService(
            ILogger<OnInitService> logger,
            ISchedueledTaskService schedueledTaskService,
            ITaskActionService taskActionService
           )
        {
            _logger = logger;
            _schedueledTaskService = schedueledTaskService;
            _taskActionService = taskActionService;
        }

        public async Task DoOverDueTasks()
        {
            var completedTasks = new List<SchedueledTask>();
            var tasks = await _schedueledTaskService.GetTaskOverDue();
            if (tasks == null || !tasks.Any())
            {
                return;
            }

            foreach (var task in tasks)
            {
                try
                {
                    await _taskActionService.DoAction(task);
                    completedTasks.Add(task);
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Something went wrong when trying to do action on init with action id ${task.Id}", ex);
                }
            }

            await _schedueledTaskService.MarkTasksAsCompleted(completedTasks);
        }
    }
}