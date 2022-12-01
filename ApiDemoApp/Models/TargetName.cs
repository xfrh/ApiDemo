namespace ApiDemoApp.Models
{
    public class TargetName
    {
        public string? point { get; set; }
    }

    public class MapName
    {
        public string? name { get; set; }
    }

    public class HostName
    {
        public string? hostname { get; set; }
    }

    public class VersionName
    {
        public string? version { get; set; }
    }
    public class ModeName
    {
        public int mode { get; set; }
    }

    

    public class TargetPolygon
    {
        public string name { get; set; }
        public double speed { get; set; }
        public List<List<double>> polygon { get; set; }
    }

    public class StatusPair : EventArgs
    {
        public string agv_name { get; set; }
        public MoveStatus status { get; set; }
    }

}
