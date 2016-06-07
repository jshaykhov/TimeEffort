using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeEffortCore.Entities;

namespace TimeEffortCore.Services
{
    public class AccessDBService
    {
        private time_trackerEntities1 db;
        public AccessDBService()
        {
            db = new time_trackerEntities1();
        }

        public bool IsInvolved(int userId, int projectId)
        {
            return db.Access.Any(x => x.UserID == userId && x.ProjectID == projectId);
        }
        public bool IsInvolved(string username, int projectId)
        {
            return db.Access.Any(x => x.UserInfo.Username == username && x.ProjectID == projectId);
        }

        public List<Project> GetProjectsByUser(string username)
        {
            List<Project> returningList = new List<Project>();

            var user = db.UserInfo.FirstOrDefault(u => u.Username == username);
            var projects = db.Project.ToList();
            
            
            foreach (Project p in projects)
            {
                if (IsInvolved(user.ID, p.ID) && p.ManagerID != user.ID)
                    returningList.Add(p);
            }

            return returningList;

        }

    }
}
