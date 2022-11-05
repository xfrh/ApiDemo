using ApiDemoApp.Models;

namespace ApiDemoApp.Services
{
    public class StatuService : BackgroundService
    {
        public static event Func<AGVCompound, Task> UpdateEvent;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000);
                if(UpdateEvent != null)
                {
                      AGVCompound aGV = new AGVCompound()
                       {
                        battery = await HttpService.Execute_Get("base_encode"),
                        timer=DateTime.Now
                       };
                    await UpdateEvent.Invoke(aGV);
                }
              

            }
        }
    }
}
