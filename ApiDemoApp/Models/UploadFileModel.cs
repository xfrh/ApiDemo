namespace ApiDemoApp.Models
{
    public class UploadFileModel
    {
        public string FileName { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
