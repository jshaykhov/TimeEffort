using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeEffortCore.Entities;

namespace TimeEffortCore.Services
{
    public class WorkloadDbService
    {
        private static time_trackerEntities1 db;

        public WorkloadDbService()
        {
            db = new time_trackerEntities1();
        }

        public void Insert(Workload item)
        {
            if (item == null)
                throw new ArgumentNullException("You cannot insert a null Insert");

            db.Workload.Add(item);
            db.SaveChanges();
        }

        public List<Workload> GetAll()
        {
                return db.Workload.ToList();
        }

        public List<Workload> GetAllbyUserAndDate(int userId, DateTime date)
        {
            return db.Workload.Where(w => w.UserID == userId && w.Date == date).ToList();
        }

        public Workload GetById(int id)
        {
            var item = db.Workload.FirstOrDefault(w => w.ID == id);
            if (item == null)
                throw new ArgumentNullException("Workload does not exist");
            return item;
        }
        public UserInfo GetByName(string name)
        {
            var item = db.UserInfo.FirstOrDefault(u => u.Username == name);
            if (item == null)
                throw new ArgumentNullException("Workload does not exist");
            return item;
        }

        public List<Workload> GetWorkloadsByUser(string name)
        {
            var user = GetByName(name);
            return db.Workload.Include("WorkloadType").Include("Project").Where(w => w.UserID == user.ID).ToList();
        }

        public void UpdateApproveStatus(int id, bool master, bool pm, bool cto)
        {
            var dbItem = db.Workload.FirstOrDefault(w => w.ID == id);
            if (dbItem == null)
                throw new ArgumentNullException("Workload does not exist");

            db.SaveChanges();
        }
        public void Update(Workload item)
        {
            var dbItem = db.Workload.FirstOrDefault(w => w.ID == item.ID);
            if (dbItem == null)
                throw new ArgumentNullException("Workload does not exist");

            dbItem.Date = item.Date;
            dbItem.UserID = item.UserID;
            dbItem.ProjectID = item.ProjectID;
            dbItem.Duration = item.Duration;
            dbItem.Note = item.Note;
            dbItem.WorkloadTypeID = item.WorkloadTypeID;

            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = db.Workload.FirstOrDefault(w => w.ID == id);
            if (item == null)
                throw new ArgumentNullException("You cannot delete a null service");
            db.Workload.Remove(item);
            db.SaveChanges();
        }

        //Project
        public List<Project> GetAllProjects()
        {
            return db.Project.ToList();
        }

        //WorkloadType
        public List<WorkloadType> GetAllTypes()
        {
            return db.WorkloadType.ToList();
        }

        //User
        public int GetUserByUsername(string username)
        {
            var user = db.UserInfo.FirstOrDefault(u => u.Username == username);
            if (user == null)
                throw new ArgumentNullException("User not found");
            return user.ID;
        }

        public List<Project> GetAllInvolvedUserPMProjects(string username)
        {
            var user = db.UserInfo.FirstOrDefault(u => u.Username == username);
            List<Project> list = new List<Project>();
            foreach (Access item in db.Access.Where(x => x.UserID == user.ID).ToList())
            {
                list.Add(item.Project);
            }
            foreach (Project item in db.Project.Where(x => x.ManagerID == user.ID).ToList())
            {
                list.Add(item);
            }

            return list;
        }

        public List<Project> GetAllInvolvedUserPMProjects(string username, DateTime from, DateTime to)
        {
            var user = db.UserInfo.FirstOrDefault(u => u.Username == username);
            List<Project> list = new List<Project>();
            foreach (Access item in db.Access.Where(x => x.UserID == user.ID && x.DateFrom >= from && x.DateTo <= to).ToList())
            {
                list.Add(item.Project);
            }
            foreach (Project item in db.Project.Where(x => x.ManagerID == user.ID && !x.Status.Equals("Completed") && x.EndDate >= to).ToList())
            {
                list.Add(item);
            }

            return list;
        }

        public bool IsAccessibleOnDate(DateTime date, int projectId, int userId)
        {
            if (db.Access.Any(x => x.UserID == userId && x.ProjectID == projectId && x.DateFrom >= date && x.DateTo <= date))
                return true;
            else
                return db.Project.Any(x => x.ManagerID == userId && x.StartDate >= date && x.EndDate <= date);
        }

        public Project GetProjectByCode(string project)
        {
            return db.Project.FirstOrDefault(p => p.Code.Equals(project));
        }
    }
}
