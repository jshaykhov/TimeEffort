using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeEffortCore.Entities;

namespace TimeEffortCore.Services
{
    public class ProjectDBService
    {
        protected static time_trackerEntities1 db;

        public ProjectDBService()
        {
            db = new time_trackerEntities1();
        }
        public void Delete(int itemId)
        {
            var item = db.Project.FirstOrDefault(p => p.ID == itemId);
            if (item == null)
                throw new ArgumentNullException("You cannot delete a project");
            db.Project.Remove(item);
            //DeleteNotification(,itemId);
            db.SaveChanges();
        }

        public List<Project> GetAll()
        {
            return db.Project.ToList();
        }

        public Project GetById(int Id)
        {
            var item = db.Project.FirstOrDefault(p => p.ID == Id);
            if (item == null)
                throw new ArgumentNullException("Project does not exist");
            return item;
        }

        public void Insert(Project item)
        {
            //try
            //{
                db.Project.Add(item);
                GenerateNotification(item);
                db.SaveChanges();
            //}
            //catch (Exception e)
            //{

            //}
        }

        public void Update(Project item)
        {

            var dbItem = db.Project.FirstOrDefault(p => p.ID == item.ID);
            if (dbItem == null)
                throw new ArgumentNullException("Project does not exist");
           
            dbItem.Name = item.Name;
            dbItem.ContractUSD = item.ContractUSD;
            dbItem.ContractUZS = item.ContractUZS;
            dbItem.ManagerID = item.ManagerID;
            dbItem.StartDate = item.StartDate;
            dbItem.EndDate = item.EndDate;
            dbItem.Status = item.Status;

            db.SaveChanges();
        }

        //users
        public List<UserInfo> GetAllUsers()
        {
            return db.UserInfo.ToList();
        }
        public List<Customer> GetAllCustomers()
        {
            return db.Customer.ToList();
        }

        public List<Project> GetProjectsByManager(int projectManagerId)
        {
            return db.Project.Where(p => p.ManagerID == projectManagerId).ToList();
        }

        public Project GetProjectByCode(string code)
        {
            return db.Project.FirstOrDefault(p => p.Code.Equals(code));
        }


        //CODE GENERATION

        public List<string> GetNextCode()
        {
            var p = db.Project.AsEnumerable().LastOrDefault();
            string i = "";
            List<string> returning = new List<string>();
            if (p != null)
                i = p.Code.Substring(8, 3);
            int incrementor = 0;
            bool successful = int.TryParse(i, out incrementor);
            if (successful)
            {
                incrementor +=1;
                i = incrementor.ToString();
                if (i.Length == 1)
                    i = "00" + incrementor;
                else if (i.Length == 2)
                    i =  "0" + incrementor;
                else if (i.Length == 3)
                    i = incrementor.ToString();
                else
                    i = "";

                if (i != "") { 
                    returning.Add("PJ" + " " + DateTime.Now.Year + "-" + i + "-R");
                    returning.Add("PJ" + " " + DateTime.Now.Year + "-" + i + "-H");
                    returning.Add("PJ" + " " + DateTime.Now.Year + "-" + i + "-M");
                    returning.Add("PJ" + " " + DateTime.Now.Year + "-" + i + "-B");
                }
                return returning;
            }
            else
                return returning;
        }
        //GENERATE NOTIFICATION
        public void GenerateNotification(Project project){

            var notification = new Notification();
            if (DateTime.Today < project.StartDate && DateTime.Today < project.EndDate)
            {
                db.Notification.Add(notification);
                notification.MESSAGE = "Project Status is Preparing";

                notification.ISREAD = false;
                db.SaveChanges();
            }
            if (DateTime.Today == project.StartDate)
            {
                db.Notification.Add(notification);
                notification.MESSAGE = "Project Status is Active ";
                notification.ISREAD = false;
                db.SaveChanges();
            }
            if(DateTime.Today==project.EndDate)
            db.Notification.Add(notification);
            notification.MESSAGE = "Project Status is Completed";
            notification.ISREAD = false;
            db.SaveChanges();
            if (DateTime.Today > project.StartDate&& DateTime.Today<project.EndDate)
            {
                db.Notification.Add(notification);
                notification.MESSAGE = "Project Status is Active";
                notification.ISREAD = false;
                db.SaveChanges();
            }
           
          }
        //UPDATE NOTIFICATION
        public void UpdateNotification(Notification notification)
        {
            var dbItem = db.Notification.FirstOrDefault(u => u.ID == notification.ID);
            //think
        }

         //DELETE NOTIFICATION
        public void DeleteNotification(int notificationId, int projectId)
        {
            var project = db.Project.FirstOrDefault(k => k.ID == projectId);
            var notification = db.Notification.FirstOrDefault(a => a.ID == notificationId);
            if (notification == null)
                throw new ArgumentNullException("You cannot delete a notification");
            db.Notification.Remove(notification);
            db.SaveChanges();
        }

    }

}



