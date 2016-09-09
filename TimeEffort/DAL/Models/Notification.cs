using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeEffort.Models
{
    public class Notification
    {
        public int ID { get; set; }
        public Nullable<int> FROMID { get; set; }
        public Nullable<int> TOID { get; set; }
        public System.DateTime Date { get; set; }
        public string MESSAGE { get; set; }
        public bool ISREAD { get; set; }
        public Nullable<int> TYPEID { get; set; }
        public int ProjectId { get; set; }


        [ForeignKey("FROMID")]
        public virtual UserInfo UserInfo { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("TOID")]
        public virtual UserInfo UserInfo1 { get; set; }

        [ForeignKey("TYPEID")]
        public virtual NotificationType NotificationType { get; set; }
    }
}