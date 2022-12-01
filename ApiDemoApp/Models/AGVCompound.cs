namespace ApiDemoApp.Models
{
    public class AGVCompound : EventArgs
    {
        public Battery battery { get; set; }
        public DateTime timer { get; set; }
      
    }
}
