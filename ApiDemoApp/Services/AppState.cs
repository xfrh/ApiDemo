using ApiDemoApp.Models;

namespace ApiDemoApp.Services
{
    public class AppState
    {
        public RegisterAGVFrom SelectedModel { get; set; }
        public ActionType SelectedType { get; private set; }
        public AGVSpeed SelectedAGVSpeed { get; set; }
        public Coordinace Nav_Coordiance { get; set; }
        public List<Coordinace> DrawCoordinates { get; set; } = new List<Coordinace>();

        public event Action OnChange;

        public void SetModel(RegisterAGVFrom model,ActionType type)
        {
            SelectedModel = model;
            SelectedType = type;
            NotifyStateChanged();
        }


         private void NotifyStateChanged() => OnChange?.Invoke();
    }

    public enum ActionType
    {
        ADD =1,
        UPDATE,
        DELETE,
        SELECTED,
        NAVIGATE,
        CANCEL,
        CHARGE,
        MANUL,
        STOPPED
    }
}
