using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeEffort.Models
{
    public class Project
    {
        public Project()
        {
            this.Access = new List<Access>();
            this.Notification = new List<Notification>();
            this.Workload = new List<Workload>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal ContractUSD { get; set; }
        public decimal ContractUZS { get; set; }
        public int ManagerID { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string CType { get; set; }
        public Nullable<int> CustomerId { get; set; }


        [InverseProperty("Project")]
        public virtual ICollection<Access> Access { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [InverseProperty("Project")]
        public virtual ICollection<Notification> Notification { get; set; }

        [ForeignKey("ManagerID")]
        public virtual UserInfo UserInfo { get; set; }

        [InverseProperty("Project")]
        public virtual ICollection<Workload> Workload { get; set; }
    }
}