using Microsoft.EntityFrameworkCore;
using WebApi.Helpers;
using WebApplication1;

namespace WebApi.Services
{
    public interface ISchedueledTaskService
    {
        Task<SchedueledTask> GetTaskById(long id);
        Task<SchedueledTask> Create(SchedueledTask schedueledTask);
        Task<SchedueledTask> MarkTaskAsCompleted(long id);
        Task MarkTasksAsCompleted(List<SchedueledTask> tasks);
        Task<IEnumerable<SchedueledTask>> GetTaskOverDue();
    }

    public class SchedueledTaskService : ISchedueledTaskService
    {
        private DataContext _context;
        private readonly ILogger<ISchedueledTaskService> _logger;
        public SchedueledTaskService(
            ILogger<SchedueledTaskService> logger,
            DataContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<SchedueledTask> GetTaskById(long id)
        {
            var task = await getTask(id);
            return task;
        }

        public async Task<SchedueledTask> Create(SchedueledTask model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Scheduled task cannot be null");
            }


            // map model to new user object
            //var schedueledTask = _mapper.Map<SchedueledTask>(model);

            model.IsCompleted = false;

            _context.SchedueledTasks.Add(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<SchedueledTask> MarkTaskAsCompleted(long id)
        {
            var task = await getTask(id);
            task.IsCompleted = true;

            _logger.LogInformation($"Task #{task.Id} has completed");

            _context.Update(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<IEnumerable<SchedueledTask>> GetTaskOverDue()
        {
            var tasks = await _context.SchedueledTasks.Where(x => !x.IsCompleted && x.FireEventTime <= DateTime.UtcNow).ToListAsync();
            return tasks;
        }

        public async Task MarkTasksAsCompleted(List<SchedueledTask> tasks)
        {
            tasks.ForEach(x => x.IsCompleted = true);
            _context.UpdateRange(tasks);
            await _context.SaveChangesAsync();
        }

        // helper methods

        private async Task<SchedueledTask> getTask(long id)
        {
            var task = await _context.SchedueledTasks.FirstOrDefaultAsync(x => x.Id == id);
            return task;
        }
    }
}