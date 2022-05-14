using ApiDemoApp.Models;

namespace ApiDemoApp.Repositories
{
    public class UserRepository
    {
        public static List<User> Users = new()
        {
            new() { Username = "frank_admin", EmailAddress = "frank.admin@email.com", Password = "PassWord", GivenName = "Frank", Surname = "Huang", Role = "Admin" },
            new() { Username = "frank_standard", EmailAddress = "frank.standard@email.com", Password = "PassWord", GivenName = "Elyse", Surname = "Burton", Role = "User" },
        };
    }
}
