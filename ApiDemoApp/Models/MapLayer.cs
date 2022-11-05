namespace ApiDemoApp.Models
{
    public class MapLayer
    {
        public int width { get; set; }
        public int height { get; set; }
        public double resolution { get; set; }
        public double origin_x { get; set; }
        public double origin_y { get; set; }
        public string? image_url { get; set; }
    }
}

 
