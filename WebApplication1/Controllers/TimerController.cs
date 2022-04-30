using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApplication1.Models.Requests;
using WebApplication1.Models.Responses;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimersController : ControllerBase
    {
        private ISchedueledTaskService _schedueledTaskService;
        private ITimedHostedService _timedHostedService;
        private ICacheService _cacheService;

        public TimersController(ISchedueledTaskService schedueledTaskService, 
                                ITimedHostedService timedHostedService, ICacheService cacheService)
        {
            _schedueledTaskService = schedueledTaskService;
            _timedHostedService = timedHostedService;
            _cacheService = cacheService;
        }

        [HttpGet("{id}")]
        public async Task<TimeLeft> Get(long id)
        {
            var task = _cacheService.GetTaskById(id);
            if (task == null)
            {
                task = await _schedueledTaskService.GetTaskById(id);
                if (task == null)
                {
                    throw new ArgumentException("No such id exists");
                }

                if (task.IsCompleted)
                {
                    throw new ArgumentException("Task is already completed");
                }
            }

            var now = DateTime.UtcNow;
            var timeLeft = (int)(task.FireEventTime - now).TotalSeconds;

            return new TimeLeft { Id = id, TImeLeft = timeLeft };
        }

        [HttpPost]
        public async Task<TaskCreated> Post(UrlTask urlTask)
        {
            if (urlTask.Hours < 0 || urlTask.Minutes < 0 || urlTask.Seconds < 0)
            {
                throw new ArgumentException("Please provide valid and larger then 0 values");
            }

            if (!Uri.IsWellFormedUriString(urlTask.Url, UriKind.Absolute))
            {
                throw new ArgumentException("Please provide a valid URL");
            }

            var fireEventTime = DateTime.UtcNow;
            fireEventTime = fireEventTime.AddHours(urlTask.Hours);
            fireEventTime = fireEventTime.AddMinutes(urlTask.Minutes);
            fireEventTime = fireEventTime.AddSeconds(urlTask.Seconds);

            var task = new SchedueledTask { Url = urlTask.Url, FireEventTime = fireEventTime };

            task = await _schedueledTaskService.Create(task);
            await _timedHostedService.SetTimer(task);
            _cacheService.Add(task);

            return new TaskCreated { Id = task.Id };
        }
    }
}