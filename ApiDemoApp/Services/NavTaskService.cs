using ApiDemoApp.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;

namespace ApiDemoApp.Services
{
    public class NavTaskService : BackgroundService
    {
        public static Dictionary<string, Queue<string>> routes = new Dictionary<string, Queue<string>>();
        public static event Func<MoveStatus, Task> UpdateStatus;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var key in routes.Keys)
                {
                    if (routes[key].Count > 0)
                    {
                        try
                        {
                            UpdateStatus?.Invoke(new MoveStatus()
                            {
                                Name = key,
                                status = await HttpService.Execute_Get("movebase_status", key)
                            });
                        }
                        catch (Exception ex)
                        {
                        }
                       
                    }
                }
                await Task.Delay(1000);
            }
        }
    }
}
