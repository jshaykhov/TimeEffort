using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TimeEffort.Models
{
    public class ChangePasswordViewModel
    {
        

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }


        [Required]
        [CompareAttribute("NewPassword", ErrorMessage = "Repeat password mismatch")]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Repeat password")]
        public string RepeatPassword { get; set; }


    }
}