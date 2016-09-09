using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeEffort.Models
{
    public class Role
    {
        public Role()
        {
            this.Access = new List<Access>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<Access> Access { get; set; }
    }
}