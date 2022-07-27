using ApiDemoApp.Models;

namespace ApiDemoApp.Services
{
    public class AppState
    {
        public RegisterAGVFrom SelectedModel { get; set; }
        public ActionType SelectedType { get; private set; }

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
        UPDATE =2,
        DELETE =3,
        SELECTED=4,
        NAVIGATE=5,
        STOP=6,
        CHARGE=7,
    }
}
