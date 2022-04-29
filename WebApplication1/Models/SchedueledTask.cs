namespace WebApplication1
{
    public class SchedueledTask
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public DateTime FireEventTime { get; set; }
        public bool IsCompleted { get; set; }
    }
}