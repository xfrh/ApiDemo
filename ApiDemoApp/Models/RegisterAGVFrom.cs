using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiDemoApp.Models
{
    public class RegisterAGVFrom : INotifyPropertyChanged
    {
        [Required]
        public string name { get; set; }

        [Required]
        public string type { get; set; }

        [Required]
        public string url { get; set; }

        private string _current_target;

        public string cur_target
        {
            get { return _current_target; }
            set { 
                _current_target = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_current_target));
            }
        }


        public AGVSpeed SelectedAGVSpeed { get; set; }

        public List<Coordinace> DrawCoordinates { get; set; }

        public int Mark { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
