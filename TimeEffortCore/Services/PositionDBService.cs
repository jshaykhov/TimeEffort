using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeEffortCore.Entities;

namespace TimeEffortCore.Services
{
    public class PositionDBService
    {
        private time_trackerEntities1 db;
        public PositionDBService()
        {
            ContextSet();
        }

        private void ContextSet()
        {
            
            db = new time_trackerEntities1();
        }
        public void Delete(int itemId)
        {
            var item = db.Position.FirstOrDefault(p => p.ID == itemId);
            if (item == null)
                throw new ArgumentNullException("You cannot delete a position");
            
            db.Position.Remove(item);
            try { 
                db.SaveChanges();
            }
            catch (Exception e)
            {
                db.Dispose();
                ContextSet();
                throw new Exception("You cannot delete this record.");
            }
        }

        public List<Position> GetAll()
        {
            return db.Position.ToList();
        }

        public Position GetById(int Id)
        {
            var item = db.Position.FirstOrDefault(p => p.ID == Id);
            if (item == null)
                throw new ArgumentNullException("Position does not exist");
            return item;
        }

        public void Insert(Position item)
        {
            db.Position.Add(item);
            db.SaveChanges();
        }

        public void Update(Position item)
        {
            var dbItem = db.Position.FirstOrDefault(p => p.ID == item.ID);
            if (dbItem == null)
                throw new ArgumentNullException("Position does not exist");
            dbItem.Name = item.Name;

            db.SaveChanges();
        }
    }
}
