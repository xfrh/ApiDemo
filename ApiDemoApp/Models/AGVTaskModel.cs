namespace ApiDemoApp.Models
{
    public class AGVTaskModel
    {
        public string AGV_No { get; set; }
        public string Title { get; set; }
        public bool CycleStyle { get; set; }
        public TimeSpan? StartTime { get; set; }
        
        public IEnumerable<string> WeekOptions { get; set; } = new HashSet<string>() { "周一" };
        public IEnumerable<string> TargetNames { get; set; } = new HashSet<string>() { "充电桩" };
        public bool AfterTask { get; set; }
    }
}
