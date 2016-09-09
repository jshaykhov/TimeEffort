using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeEffort.Models
{
    public class Customer
    {
        public Customer()
        {
            this.Project = new List<Project>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string TIN { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public string GenDirector { get; set; }
        public string ContactPerson { get; set; }

        [InverseProperty("Customer")]
        public virtual ICollection<Project> Project { get; set; }
    }
}