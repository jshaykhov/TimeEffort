using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                
                for(DateTime i = new DateTime(date.Year,date.Month,1); i<=DateTime.Today; i.AddDays(1)){
                    var temp = Service.GetAllbyUserAndDate(user.ID,i);
                    if(temp.Count ==0)
                        tempUandW.AddDay(i);
                }
                absentWorkloads.Add(tempUandW); //!!!!!!!!!!! is the list of users; for each user there is list of dates (only current month) when he didn't add any workload
            }
        }
    }
    private class UsersAndWorkloads
    {
        public UsersAndWorkloads(UserInfo _user)
        {
            user = _user;
        }
        public void AddDay(DateTime day)
        {
            days.Add(day);
        }
        UserInfo user;
        List<DateTime> days;
    }
}