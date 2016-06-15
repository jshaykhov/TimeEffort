using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeEffortCore.Entities;

namespace TimeEffortCore.Services
{
    public class WloadTypeDBService
    {
        private time_trackerEntities1 db;
        public WloadTypeDBService()
        {
            ContextSet();
        }

        private void ContextSet()
        {
            
            db = new time_trackerEntities1();
        }
        public void Delete(int itemId)
        {
            var item = db.WorkloadType.FirstOrDefault(p => p.ID == itemId);
            if (item == null)
                throw new ArgumentNullException("You cannot delete a type");
            db.WorkloadType.Remove(item);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                db.Dispose();
                ContextSet();
                throw new Exception("You cannot delete this record.");
            }
        }

        public List<WorkloadType> GetAll()
        {
            return db.WorkloadType.ToList();
        }

        public WorkloadType GetById(int Id)
        {
            var item = db.WorkloadType.FirstOrDefault(p => p.ID == Id);
            if (item == null)
                throw new ArgumentNullException("Type does not exist");
            return item;
        }

        public void Insert(WorkloadType item)
        {
            db.WorkloadType.Add(item);
            db.SaveChanges();
        }

        public void Update(WorkloadType item)
        {
            var dbItem = db.WorkloadType.FirstOrDefault(p => p.ID == item.ID);
            if (dbItem == null)
                throw new ArgumentNullException("Type does not exist");
            dbItem.Name = item.Name;

            db.SaveChanges();
        }
    }
}
