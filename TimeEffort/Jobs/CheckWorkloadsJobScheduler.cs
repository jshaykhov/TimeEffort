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
            //    .WithDailyTimeIntervalSchedule
            //      (s =>
            //         s.WithIntervalInSeconds(50)//s.WithIntervalInHours(24)
            //        .OnEveryDay()
            //        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(17, 45))
            //      )
            //    .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                             .StartNow()
                                             .WithDailyTimeIntervalSchedule(s =>
                                                 s.WithIntervalInHours(24)
                                                 .OnEveryDay()
                                                 .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(18, 45))
                                                 )
                                             .Build();
            scheduler.ScheduleJob(job, trigger);
        }
    }
}