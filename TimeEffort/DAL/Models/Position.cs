using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeEffort.Models
{
    public class Position
    {
        public Position()
        {
            this.UserInfo = new List<UserInfo>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        [InverseProperty("Position")]
        public virtual ICollection<UserInfo> UserInfo { get; set; }
    }
}