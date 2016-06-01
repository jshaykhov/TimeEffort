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
        [DataType(DataType.EmailAddress)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(maximumLength: 13, MinimumLength = 1)]
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        public int PositionId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Position")]
        public string Position { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Repeat password")]
        public string RepeatPassword { get; set; }
    }
}