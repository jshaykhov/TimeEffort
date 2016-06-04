using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TimeEffortCore.Entities;

namespace TimeEffort.Models
{
    public class MonitorViewModel
    {
        public ProjectMontior projects { get; set; }
        public EmployeeMonitor employees { get; set; }
        public QueryMonitor query { get; set; }
    }

    public class ProjectMontior
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Employee")]
        public string Employee { get; set; }

        [DataType(DataType.Text)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Type")]
        public string Type { get; set; }

        [DataType(DataType.Date)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Date")]
        public string Date { get; set; }

        [DataType(DataType.Duration)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Duration")]
        public string Duration { get; set; }

    }

    public class EmployeeMonitor
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Project")]
        public string Project { get; set; }

        [DataType(DataType.Text)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Type")]
        public string Type { get; set; }

        [DataType(DataType.Date)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Date")]
        public string Date { get; set; }

        [DataType(DataType.Duration)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Duration")]
        public string Duration { get; set; }
    }

    public class QueryMonitor
    {
        public UserInfo Employee { get; set; }
        public Project Project { get; set; }

        [DataType(DataType.Date)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [DataType(DataType.Date)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
    }


}