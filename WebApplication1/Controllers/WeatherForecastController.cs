using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimersController : ControllerBase
    {
        private static List<SchedueledTask> SchedueledTasks { get; set; } = new List<SchedueledTask>();
        private static long Id { get; set; } = 1;

        private readonly ILogger<TimersController> _logger;

        public TimersController(ILogger<TimersController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public TimeLeft Get(int id)
        {
            var task = SchedueledTasks.FirstOrDefault(x => x.Id == id);
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
        public SchedueledTask Post(UrlTask urlTask)
        {
            var fireEventTime = DateTime.UtcNow;
            fireEventTime = fireEventTime.AddHours(urlTask.Hours);
            fireEventTime = fireEventTime.AddMinutes(urlTask.Minutes);
            fireEventTime = fireEventTime.AddSeconds(urlTask.Seconds);

            var task = new SchedueledTask { Id = Id++, Url = urlTask.Url, FireEventTime = fireEventTime };
            SchedueledTasks.Add(task);

            return task;
        }
    }
}