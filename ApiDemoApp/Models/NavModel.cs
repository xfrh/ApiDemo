namespace ApiDemoApp.Models
{
    public class NavModel :EventArgs
    {
        public int Status { get; set; }
        public string? Url { get; set; }
        public string? AgvName { get; set; }
        public string? TargetName { get; set; }
        public string? JobName { get; set; }
        public int Mark { get; set; }
     }
}
