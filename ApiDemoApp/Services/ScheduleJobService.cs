
using ApiDemoApp.Events;
using ApiDemoApp.Models;
using Quartz;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;

namespace ApiDemoApp.Services
{
    public class ScheduleJobService : IJob
    {
        string Base_URL;
        string after_target;
        MoveStatus moveStatus;
        Queue<string> routes;
        EventBase eventBase;
        List<AGVProperties> lst_properties;
        Charge charge = new Charge()
        {
            type = 1,
            point = "充电桩"
        };
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                routes = new Queue<string>();
                moveStatus = new MoveStatus();
                eventBase = new EventBase();
                Coordinace start_point = await HttpService.Execute_Get("pose");
                JobKey key = context.JobDetail.Key;
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                AGVTaskModel data = JsonSerializer.Deserialize<AGVTaskModel>(dataMap.GetString("data"));
                Base_URL = "http://" +  data.url;
                string target =data.TargetName;
                string title = data.Title;
                after_target = data.AfterTask? null: JsonSerializer.Serialize(start_point);
                lst_properties =JsonSerializer.Deserialize<List<AGVProperties>>(data.properties);
                string[] targets = target.Split(",");
                for (int i = 0; i < targets.Length; i++)
                {
                    var name = targets[i].Trim();
                    if (!string.IsNullOrEmpty(name))
                        routes.Enqueue(name);
                }
                if(after_target == null)
                {
                    var last = routes.Last();
                    if (last != "充电桩")
                        routes.Enqueue("充电桩");
                }
                data.routes = routes;
                
                eventBase.TriggerNavigate(data);
            }
            catch (Exception ex)
            {

                LogService.LogMessage("schedule: " + ex.Message);
            }
           
        }
    }
}
