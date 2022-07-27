
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using ApiDemoApp.Models;

namespace ApiDemoApp.Services
{
    public class ImageService
    {
       
        private readonly string _imagePath= System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\layout1.png");
        public void CreateImage(byte[] imageBytes,int width,int height)
        {
             using(Image img = Image.Load(imageBytes))
            {
                //img.Mutate(x => x
                // .Resize(width, height)
                // );
                img.Save(_imagePath);
            }
        }

        public void SetTargetPoints(List<TargetPointsModel> targetPointsModels)
        {
            using (Image image = Image.Load(_imagePath))
            {

                foreach (var item in targetPointsModels)
                {
                    IPath cur_position = new EllipsePolygon(x: (float)(item.coordinace.x * 20), y: (float)(Math.Abs(item.coordinace.y) * 20) + 15, radius: 10.0f);
                    image.Mutate(x => x.Fill(SixLabors.ImageSharp.Color.Red, cur_position));
                }

                image.Save(_imagePath);
            }
        }   
 

        
    }
}
