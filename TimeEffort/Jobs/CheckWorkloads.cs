using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using TimeEffort.Helper;
using TimeEffortCore.Entities;
using TimeEffortCore.Services;

namespace TimeEffort.Jobs
{
    public class CheckWorkloads
    {
        static AllDBServices _Service;
        private AllDBServices Service
        {
            get
            {
                if (_Service == null)
                    _Service = new AllDBServices();
                return _Service;
            }
        }

        public void Check()
        {
            List<UserInfo> users = Service.GetAllUsers();//Get all users from AllDbServices -- GetAllUsers()
            
            List<UsersAndWorkloads> absentWorkloads = new List<UsersAndWorkloads>();
            var date = DateTime.Now;
            foreach(UserInfo user in users)
            {
                UsersAndWorkloads tempUandW = new UsersAndWorkloads(user);
                
                for(DateTime i = new DateTime(date.Year,date.Month,1); i<=DateTime.Today; i = i.AddDays(1)){
                    var temp = Service.GetAllbyUserAndDate(user.ID,i);
                    if(temp.Count ==0)
                        tempUandW.AddDay(i);
                }
                absentWorkloads.Add(tempUandW); //!!!!!!!!!!! is the list of users; for each user there is list of dates (only current month) when he didn't add any workload
            }
            SendEmailsTo(absentWorkloads);
        }


        public void SendEmailsTo(List<UsersAndWorkloads> uaw)
        {
            EmailHelper emailSender = new EmailHelper();
            foreach (UsersAndWorkloads u in uaw)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("Dear <i>" + u.user.FirstName + " "+ u.user.LastName +"</i>, <br><br>");
                sb.Append("This email is to remind you that you have not filled in any workloads for following dates: <br>");

                for(int i = 0; i < u.days.Count; i++){
                    sb.Append(u.days.ElementAt(i).Day.ToString());
                    sb.Append((i == u.days.Count - 1) ?  " of " + u.days.ElementAt(i).ToString("MMMM", CultureInfo.GetCultureInfo("en-GB")) : ", ");
                }

                sb.Append("<br><br>Best regards, <br> <h4>TaPPS team.</h4>");
                sb.Append("<hr>");
                sb.AppendLine("***This email can't receive replies. If this email wasn't meant to be sent to you, or to give us feedback on this alert, please <a href='mailto:bakir@lgcns.uz' style='text-decoration: none; color:#808080;'>write</a> to project manager***");

                string text = sb.ToString();

                if (u.user.DirectHead.HasValue){
                    sb.Append("This is only for beta. The original email is to be sent to " + u.user.Email + " and CC: " + u.user.UserInfo2.Email);// !!!! Line is to be removed
                    emailSender.SendEmail(text, "rafatdin@lgcns.uz", "viktoriya@lgcns.uz"); // u.user.Email, u.user.UserInfo2.Email);
                }
                else {
                    sb.Append("This is only for beta. The original email is to be sent to " + u.user.Email);// !!!! Line is to be removed
                    emailSender.SendEmail(text, "rafatdin@lgcns.uz");
                }
            }
        }
    }
    public class UsersAndWorkloads
    {
        public UsersAndWorkloads(UserInfo _user)
        {
            user = _user;
            days = new List<DateTime>();
        }
        public void AddDay(DateTime day)
        {
            days.Add(day);
        }
        public UserInfo user;
        public List<DateTime> days;
    }
}