using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TimeEffortCore.Entities;

namespace TimeEffort.Models
{
    public class WorkloadViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }
         [Required]
        [Display(Name = "Project")]
        public int ProjectId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Duration)]
        [Display(Name = "Duration")]
        public decimal Duration { get; set; }


        [Required]
        [Display(Name = "Approved Status")]
        public bool Approved { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Task / Note")]
        public string Note { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name="Choose Type")]
        public int WorkLoadTypeId { get; set; }
        public int WorkLoadType { get; set; }

    }
}