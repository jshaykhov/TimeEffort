using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeEffort.Models
{
    public class Access
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ProjectID { get; set; }
        public int RoleID { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }

        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }

        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }

        [ForeignKey("UserID")]
        public virtual UserInfo UserInfo { get; set; }
    }
}