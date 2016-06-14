using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using TimeEffortCore.Entities;

namespace TimeEffort.Models
{
    public class WorkloadViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

         [Display(Name = "Employee")]
        public string EmployeeName { get; set; }
        [Required]
        [Display(Name = "Project")]
        public int ProjectId { get; set; }
        public string Project { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Duration)]
        [Display(Name = "Duration")]
        public decimal Duration { get; set; }


        [Required]
        [Display(Name = "Approved Status")]
        public bool ApprovedMaster { get; set; }
        public bool ApprovedCTO { get; set; }
        public bool ApprovedPM { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Task / Note")]
        public string Note { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Choose Type")]
        public int WorkLoadTypeId { get; set; }

        [Display(Name = "Workload Type")]
        public string WorkLoadType { get; set; }

    }

    public class WorkloadCreateModel {
        [Display(Name = "Type")]
        public List<WorkloadType> Types { get; set; }
        [Display(Name = "Project")]
        public List<Project> Projects { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Note")]
        public string Note{ get; set; }

        [DataType(DataType.Duration)]
        [Display(Name = "Duration")]
        public decimal Duration { get; set; }

        public List<WorkloadViewModel> Workloads { get; set; }
        public decimal Total { get; set; }
    }

    [DataContract]
    public class RequestDataJson
    {
        [DataMember(Name = "Number")]
        public string Number { get; set; }

        [DataMember(Name = "ProjectId")]
        public int? ProjectId { get; set; }

        [DataMember(Name = "Date")]
        public DateTime Date { get; set; }

        [DataMember(Name = "Duration")]
        public string Duration { get; set; }

        [DataMember(Name = "Notes")]
        public string Notes { get; set; }

        [DataMember(Name = "TypeId")]
        public int TypeId { get; set; }

        [DataMember(Name = "SelectedType")]
        public string SelectedType { get; set; }

        [DataMember(Name = "SelectedProject")]
        public string SelectedProject { get; set; }
    }


    public class CalendarReturningModel
    {
        public DateTime Monday { get; set; }
        public List<Project> Projects { get; set; }
        public List<Workload> Workloads { get; set; }
    }

    public class CalendarDetails
    {
        public DateTime Date { get; set; }
        public WorkloadType Type { get; set; }
        public decimal Duration { get; set; }
    }


    [DataContract]
    public class CalendarRequestDataJson
    {
        [DataMember(Name = "EnteredValue")]
        public string enteredValue { get; set; }

        [DataMember(Name = "Monday")]
        public DateTime monday { get; set; }

        [DataMember(Name = "WeekDate")]
        public int weekDate{ get; set; }

        [DataMember(Name = "WorkloadName")]
        public string workloadName{ get; set; }

    }


}