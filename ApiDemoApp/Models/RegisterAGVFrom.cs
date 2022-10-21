using System.ComponentModel.DataAnnotations;

namespace ApiDemoApp.Models
{
    public class RegisterAGVFrom
    {
        [Required]
        [StringLength(8, ErrorMessage = "Name length can't be more than 8.")]
        public string name { get; set; }

        [Required]
        public string type { get; set; }

        [Required]
        public string url { get; set; }

        public string cur_target { get; set; }

        public AGVSpeed SelectedAGVSpeed { get; set; }

        public List<Coordinace> DrawCoordinates { get; set; }

        public Queue<string> routes { get; set; }
    }
}
