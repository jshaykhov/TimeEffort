using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PagedList;
namespace TimeEffort.Models
{
    public class CustomerModel
    {
        public List<CustomerViewModel> CustomerList { get; set; }
        public IPagedList<CustomerViewModel> Pagination { get; set; }
    }
    public class CustomerViewModel
    {
        public int TotalPages { get; set; }
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [StringLength(maximumLength: 1000, MinimumLength = 1)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "TIN must be a natural number")]
        [StringLength(maximumLength: 9, MinimumLength = 9)]
        [Display(Name = "TIN")]
        public string TIN { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[+]([0-9]{12})$", ErrorMessage = "Not a valid Phone number. Format: +XXXXXXXXXX")]
        [Display(Name = "Contact Person Phone")]
        public string ContactPhone { get; set; }

        [DataType(DataType.Text)]
        public string Address { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "General Director")]
        public string GenDirector { get; set; }


        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }


      
    }
  
}