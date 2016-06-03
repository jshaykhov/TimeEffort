using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace TimeEffort.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Project code")]
        public string Code { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Project name")]
        public string ProjectName { get; set; }
        [Required]
        [Display(Name = "USD")]
        public decimal CMoneyUsd { get; set; }
        [Required]
        [Display(Name = "UZS")]
        public decimal CMoneyUzs { get; set; }

        [Display(Name = "Project Manager")]
        public int PManagerId { get; set; }
   
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Project Manager")]
        public string ManagerName { get; set; }
        //public string PManagerLName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Finish Date")]
        public DateTime FinishDate { get; set; }
        public string Status { get; set; }

    }
}