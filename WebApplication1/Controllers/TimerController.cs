using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimersController : ControllerBase
    {
        private ISchedueledTaskService _schedueledTaskService;
        private static List<SchedueledTask> SchedueledTasks { get; set; } = new List<SchedueledTask>();

        private readonly ILogger<TimersController> _logger;

        public TimersController(ISchedueledTaskService schedueledTaskService, ILogger<TimersController> logger)
        {
            _logger = logger;
            _schedueledTaskService = schedueledTaskService;
        }

        [HttpGet("{id}")]
        public async Task<TimeLeft> Get(int id)
        {
            //var task = SchedueledTasks.FirstOrDefault(x => x.Id == id);
            var task = await _schedueledTaskService.GetById(id);
            if (task == null)
            {
                throw new ArgumentException("No such id exists");
            }

            if (task.IsCompleted)
            {
                throw new ArgumentException("Task is already completed");
            }

            var now = DateTime.UtcNow;
            var timeLeft = (int)(task.FireEventTime - now).TotalSeconds;

            return new TimeLeft { Id = id, TImeLeft = timeLeft };
        }

        [HttpPost(Name = "PostWeatherForecast")]
        public async Task<SchedueledTask> Post(UrlTask urlTask)
        {
            var fireEventTime = DateTime.UtcNow;
            fireEventTime = fireEventTime.AddHours(urlTask.Hours);
            fireEventTime = fireEventTime.AddMinutes(urlTask.Minutes);
            fireEventTime = fireEventTime.AddSeconds(urlTask.Seconds);

            var task = new SchedueledTask { Url = urlTask.Url, FireEventTime = fireEventTime };

            task = await _schedueledTaskService.Create(task);
            SchedueledTasks.Add(task);

            return task;
        }
    }
}