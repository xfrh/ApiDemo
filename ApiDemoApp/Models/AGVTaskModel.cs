using System.ComponentModel.DataAnnotations;

namespace ApiDemoApp.Models
{
    public class AGVTaskModel : EventArgs
    {
        public string AGV_No { get; set; }
        [Required]
        [StringLength(maximumLength:8)]
        public string Title { get; set; }
     
        public bool CycleStyle { get; set; }
        public TimeSpan? StartTime { get; set; }
       
        public IEnumerable<string> WeekOptions { get; set; } = new HashSet<string>();
     
        public IEnumerable<string> TargetOptions { get; set; } = new HashSet<string>();
        
     
        public string TargetName { get; set; }
 
        public bool AfterTask { get; set; }
        public List<AGVProperties> properties { get; set; }

        public string url { get; set; }

        public List<Coordinace> DrawCoordinates { get; set; }

        public Queue<AGVProperties> routes { get; set; }
        public int Mark { get; set; }
    }
}
