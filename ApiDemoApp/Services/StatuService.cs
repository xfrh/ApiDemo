using ApiDemoApp.Models;
using System.Net.NetworkInformation;

namespace ApiDemoApp.Services
{
    public class StatuService : BackgroundService
    {
        public static event Func<AGVCompound, Task> UpdateEvent;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (!string.IsNullOrEmpty(HttpService.Base_URL))
                    {
                        AGVCompound aGV = new AGVCompound()
                        {
                            battery = await HttpService.Execute_Get("base_encode"),
                            timer = DateTime.Now,

                        };
                              await UpdateEvent?.Invoke(aGV);
                    }
                    await Task.Delay(1000); 
                }
            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
             
            }
          
        }
    }
}
