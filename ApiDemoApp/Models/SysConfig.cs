using System.ComponentModel.DataAnnotations;

namespace ApiDemoApp.Models
{
    public class SysConfig
    {
        [Required]
        public string? TargetIp { get; set; }
        [Required]
        public string? TargetPort { get; set; }
    }
}
