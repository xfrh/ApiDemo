using ApiDemoApp.Models;
using ApiDemoApp.Services;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace ApiDemoApp.Events
{
    public class EventBase
    {
        public static EventHandler<AGVTaskModel> navigateTrigger;
        public static EventHandler navigateResetTrigger;
     
        public void TriggerNavigate(AGVTaskModel model)
        {
            navigateTrigger?.Invoke(this, model);
        }
       
    }
}
