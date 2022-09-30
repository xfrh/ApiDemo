
using ApiDemoApp.Models;
using Quartz;
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
                JobKey key = context.JobDetail.Key;
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                string url = "http://" +  dataMap.GetString("url");
                string target = dataMap.GetString("target");
                if(target == "充电桩")
                {
                    using (var client = new HttpClient())
                    {
                        string call_url = url + "/cmd/charge";
                        Charge charge = new Charge()
                        {
                            type = 1,
                            point = "充电桩"
                        };
                        var stringPayload = JsonSerializer.Serialize(charge);
                        var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                        await client.PostAsync(call_url, httpContent);
                       
                    }
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        string call_url =url + "/cmd/nav_name";
                        var stringPayload = JsonSerializer.Serialize(target);
                        var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                        var httpResponse = await client.PostAsync(call_url, httpContent);
                        await httpResponse.Content.ReadAsStringAsync();
                    }

                }

               

            }
            catch (Exception ex)
            {

                LogService.LogMessage("schedule: " + ex.Message);
            }
           
        }

     

    }
}
