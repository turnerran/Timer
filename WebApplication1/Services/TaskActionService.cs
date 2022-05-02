using WebApplication1.Models.Domains;

namespace WebApi.Services
{
    public interface ITaskActionService
    {
        Task<bool> DoAction(SchedueledTask schedueledTask);
    }

    public class TaskActionService : ITaskActionService
    {
        readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ITaskActionService> _logger;

        public TaskActionService(
            IHttpClientFactory httpClientFactory, ILogger<TaskActionService> logger
        )
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<bool> DoAction(SchedueledTask schedueledTask)
        {
            try
            {
                var id = schedueledTask.Id;
                if (id <= 0)
                {
                    throw new Exception("Task id is not valid");
                }

                var url = schedueledTask.Url;
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    throw new ArgumentException("URL is not valid");
                }

                url += $"/{id}";
                using (var httpClient = _httpClientFactory.CreateClient("action"))
                {
                    var res = await httpClient.PostAsync(url, null);
                    res.EnsureSuccessStatusCode();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"failed sending task #{schedueledTask.Id}", ex);
                return false;
            }
        }

 
    }
}