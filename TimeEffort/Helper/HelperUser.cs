using System;
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
            AccessDBService db = new AccessDBService();
            return db.IsInvolved(user.ID, project.ID);
        }

        public static List<Project> GetProjectsByManager(System.Security.Principal.IPrincipal _user)
        {
            UserService db = new UserService();
            ProjectDBService p = new ProjectDBService();
            UserInfo user = db.GetUserByUsername(_user.Identity.Name);
            return p.GetProjectsByManager(user.ID);
        }

        public static List<Project> GetProjectsByWorkingUser(System.Security.Principal.IPrincipal _user)
        {
            AccessDBService db = new AccessDBService();
            return db.GetProjectsByUser(_user.Identity.Name);
        }

        public static UserInfo GetUserByName(string username)
        {
            UserService db = new UserService();
            return db.GetUserByUsername(username);
        }


        public static Project GetProjectByCode(string projectCode)
        {
            ProjectDBService db = new ProjectDBService();
            return db.GetProjectByCode(projectCode);
        }

        public static List<UserInfo> GetAllUsers()
        {
            UserService db = new UserService();
            return db.GetAll();
        }

        internal static List<WorkloadType> GetAllWorkloadTypes()
        {
            WorkloadDbService db = new WorkloadDbService();
            return db.GetAllTypes();
        }
    }
}