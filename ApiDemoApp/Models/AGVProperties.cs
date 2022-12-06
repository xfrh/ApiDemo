using System.ComponentModel.DataAnnotations;

namespace ApiDemoApp.Models
{
    public class AGVProperties
    {
        [Required]
        [Range(0,60)]
        public int StayTime { get; set; }
        public bool isUnlockOnArrial { get; set; }
        public bool isLockedOnLeave { get; set; }
       public string Targetname { get; set; }
    }
}
