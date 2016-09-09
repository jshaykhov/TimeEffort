using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeEffort.Models
{
    public class NotificationViewModel
    {
        public int ID { get; set; }
        public int? FROMID { get; set; }
        public int? TOID { get; set; }
        public DateTime Date { get; set; }
        public string MESSAGE { get; set; }
        public bool ISREAD { get; set; }
        public int? TYPEID { get; set; }
        public int ProjectId { get; set; }

        public Project Project { get; set; }
    }
}