namespace ApiDemoApp.Models
{
    public class TargetPointsModel
    {
        public string name { get; set; }
        public Coordinace coordinace { get; set; }
        public bool isChecked { get; set; } = false;
        public bool isStarted { get; set; } = false;
        public bool isPaused { get; set; } = true;
        public bool isCheckable { get; set; } = false;
    }
}
