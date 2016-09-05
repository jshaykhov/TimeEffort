using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using TimeEffort.Helper;

namespace TimeEffort.Jobs
{
    public class CheckWorkloadsJobScheduler
    {
        //public static void Start()
        public void Start()
        {
            //IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            //scheduler.Start();

            //IJobDetail job = JobBuilder.Create<CheckWorkloadsJob>().Build();
            //var runTime = new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 30, 0, new TimeSpan(5, 0, 0)); //Today at 13:45 UTC (18:45 Tashkent time)

            //ITrigger trigger = TriggerBuilder.Create()
            //                                .WithIdentity("trigger4", "group4")
            //                                .StartAt(runTime)//.StartNow() //
            //                                .WithSimpleSchedule(x => x
            //                                    .WithIntervalInHours(24)
            //                                    .RepeatForever()
            //                                    .WithMisfireHandlingInstructionFireNow()
            //                                    ).Build();

            //scheduler.ScheduleJob(job, trigger);
            EmailHelper eh = new EmailHelper();
            eh.SendDebugMail("rafatdin@lgcns.uz");

            const double interval60Minutes = 60 * 60 * 1000; // milliseconds to one hour

            Timer checkForTime = new Timer(interval60Minutes);
            checkForTime.Elapsed += new ElapsedEventHandler(checkForTime_Elapsed);
            checkForTime.Enabled = true;

        }
        void checkForTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            //if (timeIsReady(new TimeSpan(17, 45, 00)))
            //{
                //CheckWorkloads cw = new CheckWorkloads();
                //cw.Check();

                EmailHelper eh = new EmailHelper();
                eh.SendDebugMail("rafatdin@lgcns.uz");
            //}
        }

        bool timeIsReady(TimeSpan alertTime)
        {
            DateTime current = DateTime.Now;
            TimeSpan necessaryTime = alertTime - current.TimeOfDay;

            if (necessaryTime <= new TimeSpan(1, 30, 00))
                return true;
            else
                return false;
            
        }
    }
}