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


    }
}
