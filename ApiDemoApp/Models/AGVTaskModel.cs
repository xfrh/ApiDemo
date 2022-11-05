using System.ComponentModel.DataAnnotations;

namespace ApiDemoApp.Models
{
    public class AGVTaskModel
    {
        public string AGV_No { get; set; }
      
        public string Title { get; set; }
     
        public bool CycleStyle { get; set; }
        public TimeSpan? StartTime { get; set; }
       
        public IEnumerable<string> WeekOptions { get; set; } = new HashSet<string>();
     
        public IEnumerable<string> TargetOptions { get; set; } = new HashSet<string>();

     
        public string TargetName { get; set; }
 
        public bool AfterTask { get; set; }
        public string properties { get; set; }
    }
}
