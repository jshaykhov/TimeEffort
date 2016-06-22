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
            ContextSet();
        }

        private void ContextSet()
        {
            
            db = new time_trackerEntities1();
        }
        public void DeleteRole(int itemId)
        {
            var item = db.Role.FirstOrDefault(p => p.ID == itemId);
            if (item == null)
                throw new ArgumentNullException("You cannot delete a role");
            if (db.Access.FirstOrDefault(p => p.RoleID == item.ID) != null)
            {
                throw new Exception("You cannot delete this record. Project role is in use!");
            }
            db.Role.Remove(item);
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

        public List<Role> GetAllRoles()
        {
            return db.Role.ToList();
        }

        public Role GetRoleById(int Id)
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
