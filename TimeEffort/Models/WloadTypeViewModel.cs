using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TimeEffort.Models
{
    public class WloadTypeViewModel
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 50, MinimumLength = 1)]
        [Display(Name = "Workload type")]
        public string WloadType { get; set; }
    }
}