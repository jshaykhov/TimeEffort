using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using TimeEffort.Models;

namespace TimeEffort.Mappers
{
    public class CustomerMapper
    {
        public static CustomerViewModel MapCustomerToModel(Customer customer)
        {
            return new CustomerViewModel
            {
                Id = customer.ID,
                Name = customer.Name,
                TIN=customer.TIN,
                Address=customer.Address,
                ContactPhone=customer.ContactPhone,
                GenDirector=customer.GenDirector,
                ContactPerson=customer.ContactPerson
               
            };
         
        }
        public static Customer MapCustomerFromModel(CustomerViewModel model)
        {
            return new Customer
            {
                ID = model.Id,
                Name = model.Name,
                TIN=model.TIN,
                Address=model.Address,
                ContactPhone=model.ContactPhone,
                GenDirector=model.GenDirector,
                ContactPerson=model.ContactPerson
            };
        }
        public static List<CustomerViewModel> MapCustomersToModels(List<Customer> list)
        {
            return list.Select(c => new CustomerViewModel
                {
                    Id = c.ID,
                    Name = c.Name,
                    TIN = c.TIN,
                    Address = c.Address,
                    ContactPhone = c.ContactPhone,
                    GenDirector=c.GenDirector,
                    ContactPerson=c.ContactPerson
                }).ToList();
        }
    }
}