using WebApplication1;

namespace WebApi.Services
{
    public interface ITaskActionService
    {
        Task<SchedueledTask> DoAction(SchedueledTask schedueledTask);
    }

    public class TaskActionService : ITaskActionService
    {
        private ISchedueledTaskService _schedueledTaskService;
        private readonly IHttpClientFactory _httpClientFactory;

        public TaskActionService(
            ISchedueledTaskService schedueledTaskService,
            IHttpClientFactory httpClientFactory
        )
        {
            _schedueledTaskService = schedueledTaskService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<SchedueledTask> DoAction(SchedueledTask schedueledTask)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = schedueledTask.Url + $"/{schedueledTask.Id}";
       
            await httpClient.PostAsync(url, null);
            await _schedueledTaskService.MarkTaskAsCompleted(schedueledTask.Id);

            return schedueledTask;
        }
    }
}