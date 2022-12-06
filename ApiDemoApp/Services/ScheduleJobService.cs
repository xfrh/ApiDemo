
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
       
     
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                
                EventBase eventBase = new EventBase();
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                AGVTaskModel data = JsonSerializer.Deserialize<AGVTaskModel>(dataMap.GetString("data"));
                eventBase.TriggerNavigate(data);
            }
            catch (Exception ex)
            {

                LogService.LogMessage("schedule: " + ex.Message);
            }
           
        }
    }
}
