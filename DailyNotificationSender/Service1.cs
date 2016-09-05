using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.Timers;
using WindowsServiceProject1.Jobs;

namespace WindowsServiceProject1
{
    public partial class Service1 : ServiceBase
    {

        private System.Timers.Timer timer1;
        private string timeString;
        public int getCallType;

        public Service1()
        {
            InitializeComponent();
            int strTime = Convert.ToInt32(ConfigurationManager.AppSettings["callDuration"]);
            getCallType = Convert.ToInt32(ConfigurationManager.AppSettings["CallType"]);
            if (getCallType == 1)
            {
                timer1 = new System.Timers.Timer();
                double inter = (double)GetNextInterval();
                timer1.Interval = inter;
                timer1.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick);
            }
            else
            {
                timer1 = new System.Timers.Timer();
                timer1.Interval = strTime * 1000;
                timer1.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick);
            }
        }

        /////////////////////////////////////////////////////////////////////
        protected override void OnStart(string[] args)
        {
            timer1.AutoReset = true;
            timer1.Enabled = true;
            SendMailService.WriteErrorLog("Service started");
        }

        /////////////////////////////////////////////////////////////////////
        protected override void OnStop()
        {
            timer1.AutoReset = false;
            timer1.Enabled = false;
            SendMailService.WriteErrorLog("Service stopped");
        }

        /////////////////////////////////////////////////////////////////////
        private double GetNextInterval()
        {
            //timeString = ConfigurationManager.AppSettings["StartTime"];
            //DateTime t = DateTime.Parse(timeString);

            DateTime t = DateTime.Now.AddMinutes(1);
            SendMailService.WriteErrorLog(t.ToString());

            TimeSpan ts = new TimeSpan();
            int x;
            ts = t - System.DateTime.Now;
            if (ts.TotalMilliseconds < 0)
            {
                ts = t.AddMinutes(2) - System.DateTime.Now;//Here you can increase the timer interval based on your requirments.   
                
            }
            return ts.TotalMilliseconds;
        }

        /////////////////////////////////////////////////////////////////////
        private void SetTimer()
        {
            try
            {
                double inter = (double)GetNextInterval();
                timer1.Interval = inter;
                timer1.Start();
            }
            catch (Exception ex)
            {
            }
        }

        /////////////////////////////////////////////////////////////////////
        private void ServiceTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            //string Msg = "Hi ! This is DailyMailSchedulerService mail.";//whatever msg u want to send write here.  

            //SendMailService.SendEmail("rafatdin@gmail.com", "Subject", Msg);
            CheckWorkloads cw = new CheckWorkloads();
            cw.Check();

            if (getCallType == 1)
            {
                timer1.Stop();
                System.Threading.Thread.Sleep(30000); //sleep 30 minutes
                SetTimer();
            }
        }
     
    }
}
