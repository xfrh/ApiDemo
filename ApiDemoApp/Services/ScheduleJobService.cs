
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
                JobKey key = context.JobDetail.Key;
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                Base_URL = "http://" +  dataMap.GetString("url");
                string target = dataMap.GetString("target");
                after_target = dataMap.GetString("after");
                lst_properties = JsonSerializer.Deserialize<List<AGVProperties>>(dataMap.GetString("properties"));
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
                moveStatus = await HttpService.Execute_Get("movebase_status");
                if (moveStatus.status == 2) break;
                Thread.Sleep(500);
            }
            while (moveStatus.status == 1)
            {
                moveStatus = await HttpService.Execute_Get("movebase_status");
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
                var q = lst_properties.FirstOrDefault(v => v.Targetname == number);
                string paylaod = JsonSerializer.Serialize(new TargetName() { point = number });
                await HttpService.Execute_Post("nav_point", paylaod);
                if (q.isLockedOnLeave)
                    await HttpService.Execute_Post("lock", null);
                int t_no = await Report_Progress();
                if (t_no == 3)
                {
                    int wait_time = 0;
                 
                    if(q != null)
                    {
                        wait_time = q.StayTime;
                        if (q.isUnlockOnArrial)
                            await HttpService.Execute_Post("unlock", null);
                    }
                    if (wait_time > 0)
                        Thread.Sleep(wait_time*1000);
                    await ExecuteTask();

                }
                   
                else
                    break;
            }
            if (routes.Count == 0 && after_target != null)
             {
                Coordinace coordinace = JsonSerializer.Deserialize<Coordinace>(after_target);
                var stringPayload = JsonSerializer.Serialize(coordinace);
                await HttpService.Execute_Post("nav", stringPayload);
            }
        }

     
      
       

     

      

    }
}
