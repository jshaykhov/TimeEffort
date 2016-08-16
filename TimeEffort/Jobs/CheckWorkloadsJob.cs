using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TimeEffort.Helper;

namespace TimeEffort.Jobs
{
    public class CheckWorkloadsJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            CheckWorkloads cw = new CheckWorkloads();
            cw.Check();
            
        }
    }
}