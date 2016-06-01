using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TimeEffort.Models
{
    public class WorkloadViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }


        [Required]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }


        [Required]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Project")]
        public string Project { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Approved Status")]
        public bool Approved { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Task / Note")]
        public string Note { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name="Choose Type")]
        public string WorkLoadType { get; set; }

    }
}