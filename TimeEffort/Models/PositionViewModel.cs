﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TimeEffort.Models
{
    public class PositionViewModel
    {
        public int Id { get; set; }
        
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "System role")]
        public string Position { get; set; }
    }
}