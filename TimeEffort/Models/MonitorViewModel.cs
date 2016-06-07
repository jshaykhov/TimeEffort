using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TimeEffortCore.Entities;
using Newtonsoft.Json;
using System.Runtime.Serialization;
namespace TimeEffort.Models
{
    public class MonitorViewModel
    {
        public List<ProjectMontior> projects { get; set; }
        public List<EmployeeMonitor> employees { get; set; }

        public List<Project> allProjects { get; set; }
        public List<UserInfo> allEmployees { get; set; }

        public List<WorkloadType> workloads { get; set; }
        public QueryMonitor query { get; set; }
    }

    public class ProjectMontior
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Project")]
        public string Project { get; set; }

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
        public string Employee { get; set; }
        public string Project { get; set; }
        public string WorkloadType { get; set; }

        [DataType(DataType.Date)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Date")]
        public DateTime FromDate { get; set; }

        [DataType(DataType.Date)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Date")]
        public DateTime ToDate { get; set; }
    }


    [DataContract]
    public class QueryJson
    {
        [DataMember(Name = "FromDate")]
        public string FromDate { get; set; }

        [DataMember(Name = "ToDate")]
        public string ToDate { get; set; }

        [DataMember(Name = "SelectedUser")]
        public string SelectedUser { get; set; }

        [DataMember(Name = "SelectedProject")]
        public string SelectedProject { get; set; }

        [DataMember(Name = "SelectedType")]
        public string SelectedType { get; set; }
    }  
}