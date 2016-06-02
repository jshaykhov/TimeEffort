﻿using System;
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

        public void Update(Workload item)
        {
            var dbItem = db.Workload.FirstOrDefault(w => w.ID == item.ID);
            if (dbItem == null)
                throw new ArgumentNullException("Workload does not exist");

            dbItem.Date = item.Date;
            dbItem.UserID = item.UserID;
            dbItem.ProjectID = item.ProjectID;
            dbItem.Duration = item.Duration;
            dbItem.Approved = item.Approved;
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

    }
}
