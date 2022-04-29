
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Helpers;
using WebApplication1;

namespace WebApi.Services
{
    public interface ISchedueledTaskService
    {
        Task<SchedueledTask> GetById(long id);
        Task<SchedueledTask> Create(SchedueledTask schedueledTask);
        Task MarkTaskAsCompleted(long id);
        Task MarkTasksAsCompleted(List<SchedueledTask> tasks);
        Task<IEnumerable<SchedueledTask>> GetTaskOverDue();
    }

    public class SchedueledTaskService : ISchedueledTaskService
    {
        private DataContext _context;

        public SchedueledTaskService(
            DataContext context)
        {
            _context = context;
        }

        public async Task<SchedueledTask> GetById(long id)
        {
            return await getTask(id);
        }

        public async Task<SchedueledTask> Create(SchedueledTask model)
        {
            // validate
        //    if (_context.Users.Any(x => x.Email == model.Email))
           //     throw new AppException("User with the email '" + model.Email + "' already exists");

            // map model to new user object
            //var schedueledTask = _mapper.Map<SchedueledTask>(model);

            // save user
            _context.SchedueledTasks.Add(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task MarkTaskAsCompleted(long id)
        {
            var task = await getTask(id);
            task.IsCompleted = true;
            _context.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SchedueledTask>> GetTaskOverDue()
        {
            var tasks = await _context.SchedueledTasks.Where(x => !x.IsCompleted && x.FireEventTime >= DateTime.UtcNow).ToListAsync();
            return tasks;
        }

        public async Task MarkTasksAsCompleted(List<SchedueledTask> tasks)
        {
            _context.UpdateRange(tasks);
            await _context.SaveChangesAsync();
        }

        // helper methods

        private async Task<SchedueledTask> getTask(long id)
        {
            var task = await _context.SchedueledTasks.FindAsync(id);
            if (task == null)
            {
                throw new KeyNotFoundException("task not found");
            }

            return task;
        }
    }
}