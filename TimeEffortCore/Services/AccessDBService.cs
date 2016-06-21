using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeEffortCore.Entities;

namespace TimeEffortCore.Services
{
    public class AccessDBService:ProjectDBService
    {
        //private time_trackerEntities1 db;
        //public AccessDBService()
        //{
        //    db = new time_trackerEntities1();
        //}
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

        //Getting all
        public List<Access> GetAll()
        {
            return db.Access.ToList();
        }
        public List<UserInfo> GetAllUsers()
        {
            return db.UserInfo.ToList();
        }


        //nargiza
        //Project
        public List<Project> GetAllProjects()
        {
                return db.Project.ToList();
        }

        
        //User
        public int GetUserIdByUsername(string username)
        {
            var user = db.UserInfo.FirstOrDefault(u => u.Username == username);
            if (user == null)
                throw new ArgumentNullException("User not found");
            return user.ID;

        }
        public List<Role> GetAllRoles()
        {
            return db.Role.ToList();
        }
        public void Insert(Access item)
        {
            db.Access.Add(item);
            db.SaveChanges();
        }
        public Access GetById(int Id)
        {
            var item = db.Access.FirstOrDefault(p => p.ID == Id);
            if (item == null)
                throw new ArgumentNullException("Appointment does not exist");
            return item;
        }
        //EDITING
        public void Update(Access item)
        {
            var dbItem = db.Access.FirstOrDefault(p => p.ID == item.ID);
            if (dbItem == null)
                throw new Exception("Appointment does not exist");
            //dbItem.UserID = item.UserID;
            var projectItem = db.Project.FirstOrDefault(p => p.ID == item.ProjectID);

            if(item.DateFrom<projectItem.StartDate || item.DateFrom>projectItem.EndDate)
                throw new Exception("Date From should be between" + projectItem.StartDate.ToShortDateString() + " and " + projectItem.EndDate.ToShortDateString());

            if (item.DateTo < projectItem.StartDate || item.DateTo > projectItem.EndDate)
                throw new Exception("Date To should be between" + projectItem.StartDate.ToShortDateString() + " and " + projectItem.EndDate.ToShortDateString());

            if (item.DateFrom > projectItem.EndDate)
                throw new Exception("Date To cannot be earlier than Date From");
            //dbItem.ProjectID = item.ProjectID;

            dbItem.RoleID = item.RoleID;
            dbItem.DateFrom = item.DateFrom;
            dbItem.DateTo = item.DateTo;

            db.SaveChanges();
        }

        //DELETING THE APPOINTMENT
        public void Delete(int itemId)
        {
            var item = db.Access.FirstOrDefault(p => p.ID == itemId);
            if (item == null)
                throw new ArgumentNullException("You cannot delete an appointment");
            db.Access.Remove(item);
            db.SaveChanges();
        }
    }
}
