using ApiDemoApp.Models;
using Quartz;
using Quartz.Impl;
using System.Text.Json;

namespace ApiDemoApp.Services
{
    public class TimeJobService
    {
       
        public static async Task AddToSchedule(AGVTaskModel model)
        {
            try
            {
                    ISchedulerFactory sf = new StdSchedulerFactory();
                     HttpService.Base_URL =  model.url;
                    Coordinace start_point = await HttpService.Execute_Get("pose");
                        var scheduler = await sf.GetScheduler();
                    IJobDetail job = JobBuilder.Create<ScheduleJobService>()
                   .WithIdentity(model.Title, model.AGV_No)
                   .UsingJobData("data", JsonSerializer.Serialize(model))
                   .Build();

                    var triggerKey = new TriggerKey(model.Title);
                    string schedulModel = EditShowList(model);
                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity(triggerKey)
                        .StartNow()
                        .WithCronSchedule(schedulModel) 
                        .Build();
                    await scheduler.Start();
                    await scheduler.ScheduleJob(job, trigger);
               
              
            }
            catch (Exception ex)
            {

                LogService.LogMessage("AGVStatus AddtoSchedule:" + ex.Message);
            }


        }

        public static string EditShowList(AGVTaskModel _model)
        {

            string schedulModel = "0 " + _model.StartTime.Value.Minutes + " " + _model.StartTime.Value.Hours + " ? * ";
            foreach (var p in _model.WeekOptions)
            {
                switch (p)
                {
                    case "周一":
                        schedulModel += "MON,";
                        break;
                    case "周二":
                        schedulModel += "TUE,";
                        break;
                    case "周三":
                        schedulModel += "WED,";
                        break;
                    case "周四":
                        schedulModel += "THU,";
                        break;
                    case "周五":
                        schedulModel += "FRI,";
                        break;
                    case "周六":
                        schedulModel += "SAT,";
                        break;
                    case "周日":
                        schedulModel += "SUN,";
                        break;

                }
            }

            schedulModel = schedulModel.TrimEnd(',');

            return schedulModel;

        }

        public static async Task DeleteSchedule(string token)
        {
            try
            {
                ISchedulerFactory sf = new StdSchedulerFactory();
                TriggerKey triggerKey = new TriggerKey(token);
                IScheduler scheduler= await sf.GetScheduler();
                await scheduler.UnscheduleJob(triggerKey);
            }
            catch (Exception ex)
            {

                LogService.LogMessage("AGVStatus deleteschedule:" + ex.Message);
            }
        }




    }
}
