using ApiDemoApp.Models;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;



namespace ApiDemoApp.Services
{
    public class MapService
    {
       static string rootpath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot");
        public static async Task<string> ProcessMap(AGVTaskModel model)
        {

            try
            {
               
                MapLayer mapLayer = await HttpService.Execute_Get("map");
                Coordinace coordinance = await HttpService.Execute_Get("pose");
                List<TargetPointsModel> targetPointsModels = await HttpService.Execute_Get("android_target");
                List<List<double>> restrictArea = await HttpService.Execute_Get("restrict_layer");
                byte[] imageBytes = Convert.FromBase64String(mapLayer.image_url.Replace("data:image/png;base64,", "").Trim());
                FontCollection collection = new();
                FontFamily family = collection.Add(@"C:\Windows\Fonts\simhei.ttf");
                Font font = family.CreateFont(12, FontStyle.Italic);
                Font sp_font = family.CreateFont(24, FontStyle.Bold);
                using (Image img = Image.Load(imageBytes))
                {
                    img.Mutate(x => x.Resize(mapLayer.width, mapLayer.height));

                    if (targetPointsModels != null && targetPointsModels.Count > 0)
                    {
                        if (string.IsNullOrEmpty(model.TargetName))
                        {
                            foreach (var item in targetPointsModels)
                            {
                                var px = (item.coordinace.x - mapLayer.origin_x) * 20;
                                var py = mapLayer.height - ((item.coordinace.y - mapLayer.origin_y) * 20);
                                IPath cur_position = new EllipsePolygon((float)px, (float)py, radius: 10.0f);
                               // img.Mutate(x => x.Fill(SixLabors.ImageSharp.Color.Red, cur_position));
                                img.Mutate(x => x.DrawText(item.name, font, SixLabors.ImageSharp.Color.Black, new PointF((float)px, (float)py + 15)));

                                var target_image = Image.Load(rootpath + "\\images\\pointsmall.png");
                                img.Mutate(x => x.DrawImage(target_image, new Point() { X = (int)px, Y = (int)py }, 1f));

                            }

                        }

                        else
                        {
                            var item = targetPointsModels.FirstOrDefault(v => v.name == model.TargetName);
                            if (item != null)
                            {
                                var px = (item.coordinace.x - mapLayer.origin_x) * 20;
                                var py = mapLayer.height - ((item.coordinace.y - mapLayer.origin_y) * 20);
                                IPath cur_position = new EllipsePolygon((float)px, (float)py, radius: 10.0f);
                                var target_image = Image.Load(rootpath + "\\images\\pointbig.png");
                                img.Mutate(x => x.DrawImage(target_image, new Point() { X = (int)px, Y = (int)py }, 1f));
                                img.Mutate(x => x.DrawText(item.name, font, SixLabors.ImageSharp.Color.Black, new PointF((float)px+5, (float)py + 25)));
                            }
                         
                        }
                           
                    }

                    if (coordinance != null)
                    {
                        var px_xiaoche = (coordinance.x - mapLayer.origin_x) * 20; //(12.74 + coordinance.x) * 20;
                        var py_xiaoche = mapLayer.height - ((coordinance.y - mapLayer.origin_y) * 20); //(20.12 - coordinance.y) * 20;
                        //IPath cur_position_xiaoche = new EllipsePolygon((float)px_xiaoche, (float)py_xiaoche, radius: 10.0f);
                        //img.Mutate(x => x.Fill(SixLabors.ImageSharp.Color.Yellow, cur_position_xiaoche));
                        //img.Mutate(x => x.DrawText("车", font, SixLabors.ImageSharp.Color.Black, new PointF((float)px_xiaoche - 4, (float)py_xiaoche - 5)));
                        var target_image =string.IsNullOrEmpty(model.TargetName)? Image.Load(rootpath + "\\images\\carsmall.png") : Image.Load(rootpath + "\\images\\car.png");
                        img.Mutate(x => x.DrawImage(target_image, new Point() { X = (int)px_xiaoche, Y = (int)py_xiaoche }, 1f));
                        if(string.IsNullOrEmpty(model.TargetName))
                        img.Mutate(x => x.DrawText("车", font, SixLabors.ImageSharp.Color.Black, new PointF((float)px_xiaoche+5, (float)py_xiaoche + 14)));
                        else
                            img.Mutate(x => x.DrawText("车", font, SixLabors.ImageSharp.Color.Black, new PointF((float)px_xiaoche+5, (float)py_xiaoche + 28)));
                    }


                    if (restrictArea != null && restrictArea.Count > 0)
                    {
                        var linePen = new Pen(SixLabors.ImageSharp.Color.Red, 2.5f);
                        var points = new PointF[restrictArea.Count];
                        for (int i = 0; i < restrictArea.Count; i++)
                        {
                            var px = (restrictArea[i][0] - mapLayer.origin_x) * 20;
                            var py = mapLayer.height - ((restrictArea[i][1] - mapLayer.origin_y) * 20);

                            IPath yourPolygon = new Star(x: (float)px, y: (float)py, prongs: 5, innerRadii: 0.5f, outerRadii: 2.0f);
                            img.Mutate(x => x.Fill(SixLabors.ImageSharp.Color.Red, yourPolygon));

                        }
                    }
                    if (model.DrawCoordinates != null && model.DrawCoordinates.Count > 2)
                    {
                        var linePen = new Pen(SixLabors.ImageSharp.Color.Blue, 2.5f);
                        var plans = model.DrawCoordinates;
                        var points = new PointF[plans.Count];

                        for (int i = 0; i < plans.Count; i++)
                        {
                            var px = (plans[i].x - mapLayer.origin_x) * 20; //(12.74 + plans[i].x) * 20; 
                            var py = mapLayer.height - ((plans[i].y - mapLayer.origin_y) * 20);//(20.12 - plans[i].y) * 20;
                            points[i] = new PointF((float)px, (float)py);
                        }
                        img.Mutate(x => x.DrawLines(linePen, points));

                    }

                    using (MemoryStream m = new MemoryStream())
                    {

                        img.Save(m, new PngEncoder());
                        byte[] _imageBytes = m.ToArray();
                        return "data:image/png;base64," + Convert.ToBase64String(_imageBytes);
                    
                    }

                }
            }
            catch (Exception ex)
            {

                LogService.LogMessage("processmap" + ex.Message);
                return null;
            }


        }

    
    }
}
