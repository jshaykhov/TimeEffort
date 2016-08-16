using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeEffort.Jobs
{
    public class CheckWorkloadsJobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<CheckWorkloadsJob>().Build();
            
            //ITrigger trigger = TriggerBuilder.Create()
            //                                 .StartNow()
            //                                 .WithDailyTimeIntervalSchedule(s =>
            //                                     s.WithIntervalInHours(24)
            //                                     .OnEveryDay()
            //                                     .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(18, 45))
            //                                     )
            //                                 .Build();

            var runTime = new DateTimeOffset(DateTime.Today, new TimeSpan(13, 45, 0)); //Today at 13:45 UTC (18:45 Tashkent time)

            ITrigger trigger = TriggerBuilder.Create()
                                            .WithIdentity("trigger1", "group1")
                                            .StartAt(runTime)
                                            .WithSimpleSchedule(x => x
                                                .WithIntervalInHours(24)
                                                .RepeatForever())
                                            .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}