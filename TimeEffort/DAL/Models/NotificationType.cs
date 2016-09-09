using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeEffort.Models
{
    public class NotificationType
    {
        public NotificationType()
        {
            this.Notification = new List<Notification>();
        }

        public int ID { get; set; }
        public string NAME { get; set; }

        [InverseProperty("NotificationType")]
        public virtual ICollection<Notification> Notification { get; set; }
    }
}