namespace ApiDemoApp.Models
{
    public class MoveStatus : EventArgs
    {
        public int status { get; set; }
        public string Name { get; set; }
    }
}
