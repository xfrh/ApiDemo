using System.ComponentModel.DataAnnotations;
namespace ApiDemoApp.Models
{
    public class User
    {
        
        public string Username { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? GivenName { get; set; }
        public string? Surname { get; set; }
        public string? Role { get; set; }
    }
}
