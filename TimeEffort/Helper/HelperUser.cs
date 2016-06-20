﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeEffortCore.Entities;
using TimeEffortCore.Services;

namespace TimeEffort.Helper
{
    public class HelperUser
    {
        public static string GetRoleName(System.Security.Principal.IPrincipal user)
        {
            if (user.IsInRole("Admin"))
                return "Admin";
            else if (user.IsInRole("Master"))
                return "Master";
            else if (user.IsInRole("Monitor"))
                return "Monitor";
            else if (user.IsInRole("CTO"))
                return "CTO";
            else if (user.IsInRole("User"))
                return "User";
            else
                return "";
        }

        public static bool IsInvolvedInProject(Project project, UserInfo user)
        {
            using (time_trackerEntities1 ctx = new time_trackerEntities1())
            {
                return ctx.Access.Any(x => x.UserID == user.ID && x.ProjectID == project.ID);
            }
        }

        public static List<Project> GetAllInvolvedProjects(System.Security.Principal.IPrincipal _user)
        {
            using (time_trackerEntities1 ctx = new time_trackerEntities1())
            {

                var allProjects = ctx.Project.ToList(); ;
                List<Project> returningProjects = new List<Project>();
                foreach (Project p in allProjects)
                {
                    if (ctx.Access.Any(x => x.UserInfo.Username == _user.Identity.Name && x.ProjectID == p.ID))
                        returningProjects.Add(p);
                }
                return returningProjects;
            }
        }


        public static List<Project> GetProjectsByManager(System.Security.Principal.IPrincipal _user)
        {
            using (time_trackerEntities1 ctx = new time_trackerEntities1())
            {
                UserInfo user = ctx.UserInfo.FirstOrDefault(u => u.Username == _user.Identity.Name);
                return ctx.Project.Where(p => p.ManagerID == user.ID).ToList();
            }
        }

        public static List<Project> GetProjectsByWorkingUser(System.Security.Principal.IPrincipal _user)
        {
            using (time_trackerEntities1 ctx = new time_trackerEntities1())
            {
                List<Project> returningList = new List<Project>();

                var user = ctx.UserInfo.FirstOrDefault(u => u.Username == _user.Identity.Name);
                var projects = ctx.Project.ToList();


                foreach (Project p in projects)
                {
                    if (ctx.Access.Any(x => x.UserID == user.ID && x.ProjectID == p.ID) && p.ManagerID != user.ID)
                        returningList.Add(p);
                }

                return returningList;
            }
        }

        public static UserInfo GetUserByName(string username)
        {
            using (time_trackerEntities1 ctx = new time_trackerEntities1())
            {
                return ctx.UserInfo.FirstOrDefault(u => u.Username == username);
            }
        }


        public static Project GetProjectByCode(string projectCode)
        {
            using (time_trackerEntities1 ctx = new time_trackerEntities1())
            {
                return ctx.Project.FirstOrDefault(p => p.Code.Equals(projectCode));
            }
        }

        public static List<UserInfo> GetAllUsers()
        {
            using (time_trackerEntities1 ctx = new time_trackerEntities1())
            {
                return ctx.UserInfo.ToList();
            }
        }

        internal static List<WorkloadType> GetAllWorkloadTypes()
        {
            using (time_trackerEntities1 ctx = new time_trackerEntities1())
            {
                return ctx.WorkloadType.ToList();
            }
        }
    }
}