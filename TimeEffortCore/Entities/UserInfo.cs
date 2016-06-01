//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimeEffortCore.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserInfo
    {
        public UserInfo()
        {
            this.Access = new HashSet<Access>();
            this.Workload = new HashSet<Workload>();
            this.Project = new HashSet<Project>();
        }
    
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int PositionID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    
        public virtual ICollection<Access> Access { get; set; }
        public virtual Position Position { get; set; }
        public virtual ICollection<Workload> Workload { get; set; }
        public virtual ICollection<Project> Project { get; set; }
    }
}
