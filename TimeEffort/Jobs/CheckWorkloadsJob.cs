using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeEffort.Jobs
{
    public class CheckWorkloadsJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //call check.
        }
    }
}