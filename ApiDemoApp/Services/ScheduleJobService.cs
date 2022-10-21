
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
                JobKey key = context.JobDetail.Key;
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                Base_URL = "http://" +  dataMap.GetString("url");
                string target = dataMap.GetString("target");
                after_target = dataMap.GetString("after");

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
                await Task.Run(() => ExecuteTask());

            }
            catch (Exception ex)
            {

                LogService.LogMessage("schedule: " + ex.Message);
            }
           
        }

        public async Task<int> Report_Progress()
        {
            while (moveStatus.status != 1)
            {
                moveStatus = await Cur_Status();
                if (moveStatus.status == 2) break;
                Thread.Sleep(500);
            }
            while (moveStatus.status == 1)
            {
                moveStatus = await Cur_Status();
                if (moveStatus.status == 2) break;
                Thread.Sleep(500);
            }
            return moveStatus.status;

        }

        public async Task ExecuteTask()
        {
            while (routes.Count > 0)
            {
                string number = routes.Dequeue();
                await StartNav(new TargetName() { point = number });
                int t_no = await Report_Progress();
                if (t_no == 3)
                    await ExecuteTask();
                else
                    break;
            }
            if (routes.Count == 0 && after_target != null)
             {
                Coordinace coordinace = JsonSerializer.Deserialize<Coordinace>(after_target);
                await StartNav(coordinace);
            }
        }

        public async Task<MoveStatus> Cur_Status()
        {
           try
            {
                using (var client = new HttpClient())
                {
                    string call_url = Base_URL + "/reeman/movebase_status";
                    return await client.GetFromJsonAsync<MoveStatus>(call_url);
                }

            }
            catch (Exception ex)
            {
                LogService.LogMessage(ex.Message);
                return await Task.FromResult<MoveStatus>(null);
            }
        }
        public async Task<string> StartNav(Coordinace coordinace)
        {
            string statusMessage = "";
            try
            {
                using (var client = new HttpClient())
                {
                    string call_url = Base_URL + "/cmd/nav";
                    coordinace.theta = coordinace.theta * Math.PI / 180;
                    var stringPayload =JsonSerializer.Serialize(coordinace);
                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                    var httpResponse = await client.PostAsync(call_url, httpContent);
                    statusMessage = await httpResponse.Content.ReadAsStringAsync();

                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        statusMessage = "";
                    }
                    else
                    {
                        statusMessage = await httpResponse.Content.ReadAsStringAsync();
                        LogService.LogMessage(statusMessage);
                    }
                }
            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);

            }
            return statusMessage;
        }
        public async Task<string> StartNav(TargetName pointname)
        {
                 
            string statusMessage = "";
            try
            {
                 using (var client = new HttpClient())
                {
                    string target_url = pointname.point == "充电桩" ? "/cmd/charge" : "/cmd/nav_point";
                    string call_url = Base_URL + target_url;
                    var stringPayload = pointname.point == "充电桩" ? JsonSerializer.Serialize(charge) : JsonSerializer.Serialize(pointname);
                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                    var httpResponse = await client.PostAsync(call_url, httpContent);
                    statusMessage = await httpResponse.Content.ReadAsStringAsync();

                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        statusMessage = "";
                    }
                    else
                    {
                        statusMessage = await httpResponse.Content.ReadAsStringAsync();
                        LogService.LogMessage(statusMessage);
                    }
                }
            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);

            }
            return statusMessage;
        }

       

    }
}
