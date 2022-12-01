namespace ApiDemoApp.Models
{
    public class NavTaskModel
    {
        public Queue<NavModel> Routes { get; set; }
        public bool IsLockedOnLeave { get; set; }
        public bool IsUnlockOnArrival { get; set; }

    }
}
