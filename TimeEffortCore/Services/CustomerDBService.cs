﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeEffortCore.Entities;

namespace TimeEffortCore.Services
{
    public class CustomerDBService
    {
        private time_trackerEntities1 db;
        public CustomerDBService()
        {
            db = new time_trackerEntities1();
        }
        public void Delete(int itemId)
        {
            var item = db.Customer.FirstOrDefault(p => p.ID == itemId);
            if (item == null)
                throw new ArgumentNullException("You cannot delete a customer");
            db.Customer.Remove(item);
            db.SaveChanges();
        }

        public List<Customer> GetAll()
        {
            return db.Customer.ToList();
        }

        public Customer GetById(int Id)
        {
            var item = db.Customer.FirstOrDefault(p => p.ID == Id);
            if (item == null)
                throw new ArgumentNullException("Customer does not exist");
            return item;
        }

        public void Insert(Customer item)
        {
            db.Customer.Add(item);
            db.SaveChanges();
        }

        public void Update(Customer item)
        {
            var dbItem = db.Customer.FirstOrDefault(p => p.ID == item.ID);
            if (dbItem == null)
                throw new ArgumentNullException("Customer does not exist");
            dbItem.Name = item.Name;
            dbItem.Address = item.Address;
            dbItem.TIN = item.TIN;
            dbItem.Phone = item.Phone;
            db.SaveChanges();
        }
    }
}
