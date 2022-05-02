using WebApplication1.Models.Domains;

namespace WebApi.Services
{
    public interface ICacheService
    {
        SchedueledTask GetTaskById(long id);
        void Add(SchedueledTask schedueledTask);
        void Remove(long id);
    }

    public class CacheService : ICacheService
    {
        private static Dictionary<long, SchedueledTask> _cache = new Dictionary<long, SchedueledTask>();

        public void Add(SchedueledTask schedueledTask)
        {
            if (_cache.ContainsKey(schedueledTask.Id))
            {
                throw new InvalidOperationException("task already exists in the cache");
            }

            _cache.Add(schedueledTask.Id, schedueledTask);
        }

        public SchedueledTask GetTaskById(long id)
        {
            if (_cache.ContainsKey(id))
            {
                {
                    return _cache[id];
                }
            }

            return null;
        }

        public void Remove(long id)
        {
            if (_cache.ContainsKey(id))
            {
                _cache.Remove(id);
            }
        }
    }
}