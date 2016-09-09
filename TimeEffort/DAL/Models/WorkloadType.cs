using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeEffort.Models
{
    public class WorkloadType
    {
        public WorkloadType()
        {
            this.Workload = new List<Workload>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        [InverseProperty("WorkloadType")]
        public virtual ICollection<Workload> Workload { get; set; }
    }
}