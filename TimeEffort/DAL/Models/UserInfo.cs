using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeEffort.Models
{
    public class UserInfo
    {
        public UserInfo()
        {
            this.Access = new List<Access>();
            this.Notification = new List<Notification>();
            this.Notification1 = new List<Notification>();
            this.Project = new List<Project>();
            this.Workload = new List<Workload>();
            this.UserInfo1 = new List<UserInfo>();
        }

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int PositionID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Major { get; set; }
        public Nullable<int> DirectHead { get; set; }

        
        [InverseProperty("UserInfo")]
        public virtual ICollection<Access> Access { get; set; }

        [InverseProperty("UserInfo")]
        public virtual ICollection<Notification> Notification { get; set; }

        [InverseProperty("UserInfo1")]
        public virtual ICollection<Notification> Notification1 { get; set; }
        
        [ForeignKey("PositionID")]
        public virtual Position Position { get; set; }
        
        [InverseProperty("UserInfo")]
        public virtual ICollection<Project> Project { get; set; }
        
        [InverseProperty("UserInfo")]
        public virtual ICollection<Workload> Workload { get; set; }

        [InverseProperty("UserInfo2")]
        public virtual ICollection<UserInfo> UserInfo1 { get; set; }

        [ForeignKey("DirectHead")]
        public virtual UserInfo UserInfo2 { get; set; }

    }
}