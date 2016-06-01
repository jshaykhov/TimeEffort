using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeEffortCore.Entities;

namespace TimeEffortCore.Services
{
    public class RoleDBService
    {
        private time_trackerEntities1 db;
        public RoleDBService()
        {
            db = new time_trackerEntities1();
        }
        public void Delete(int itemId)
        {
            var item = db.Role.FirstOrDefault(p => p.ID == itemId);
            if (item == null)
                throw new ArgumentNullException("You cannot delete a role");
            db.Role.Remove(item);
            db.SaveChanges();
        }

        public List<Role> GetAll()
        {
            return db.Role.ToList();
        }

        public Role GetById(int Id)
        {
            var item = db.Role.FirstOrDefault(p => p.ID == Id);
            if (item == null)
                throw new ArgumentNullException("Role does not exist");
            return item;
        }

        public void Insert(Role item)
        {
            db.Role.Add(item);
            db.SaveChanges();
        }

        public void Update(Role item)
        {
            var dbItem = db.Role.FirstOrDefault(p => p.ID == item.ID);
            if (dbItem == null)
                throw new ArgumentNullException("Role does not exist");
            dbItem.Name = item.Name;

            db.SaveChanges();
        }
    }
}
