﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PagedList;
namespace TimeEffort.Models
{
    public class SendingModel
    {
        public List<UserViewModel> UserList { get; set; }
        public IPagedList<UserViewModel> Pagination { get; set; }
    }

        public class UserViewModel
    {
        public int TotalPages { get; set; }
        public int Id { get; set; }
        public string FullName { get; set; }
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
        [RegularExpression(@"^[+]([0-9]{12})$", ErrorMessage = "Not a valid Phone number. Format: +XXXXXXXXXX")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        public int PositionId { get; set; }
      
        [Display(Name = "Position")]


        public string Position { get; set; }
                
        [Required]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        


        
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}