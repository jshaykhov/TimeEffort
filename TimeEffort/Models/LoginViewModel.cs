using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TimeEffort.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    [DataContract]
    public class LoginJson
    {
        [DataMember(Name = "Username")]
        public string Username { get; set; }

        [DataMember(Name = "Password")]
        public string Password { get; set; }
        
        [DataMember(Name = "RememberMe")]
        public string RememberMe { get; set; }

        [DataMember(Name = "ReturnUrl")]
        public string ReturnUrl { get; set; }
    }  
}