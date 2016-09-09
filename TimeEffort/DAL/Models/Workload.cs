using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeEffort.Models
{
    public class Workload
    {
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public int UserID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public decimal Duration { get; set; }
        public string Note { get; set; }
        public int WorkloadTypeID { get; set; }


        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }

        [ForeignKey("UserID")]
        public virtual UserInfo UserInfo { get; set; }
        
        [ForeignKey("WorkloadTypeID")]
        public virtual WorkloadType WorkloadType { get; set; }
    }
}