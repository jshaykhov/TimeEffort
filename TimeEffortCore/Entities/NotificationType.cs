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
    
    public partial class NotificationType
    {
        public NotificationType()
        {
            this.Notification = new HashSet<Notification>();
        }
    
        public int ID { get; set; }
        public string NAME { get; set; }
    
        public virtual ICollection<Notification> Notification { get; set; }
    }
}
