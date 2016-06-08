using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TimeEffortCore.Entities;

namespace TimeEffort.Models
{
    public class PagingModel
    {
        public IPagedList<AppointViewModel> Paging { get; set; }

        public List<AppointViewModel> AppointList { get; set; }

    }
    public class AppointViewModel
    {

        public int TotalPages { get; set; }
        public int Id { get; set; }

        [Display(Name = "User ID")]     
        public int UserID { get; set; }

       
        [DataType(DataType.Text)]
        [Display(Name = "User")]
        public string User { get; set; }


        [Display(Name = "Project ID")]
        public int ProjectID { get; set; }

       
         [DataType(DataType.Text)]
         [Display(Name = "Project name")]
        public string Project { get; set; }

         [Display(Name = "Role ID")]       
        public int RoleID { get; set; }

       
        [DataType(DataType.Text)]
        [Display(Name = "Role")]
        public string Role { get; set; }

        
    }

    public class AppointCreateModel
    {
        [Display(Name = "User")]

        public List<UserInfo> Users { get; set; }


        [Display(Name = "Project")]
        public List<Project> Projects { get; set; }

        [Display(Name = "Role")]
        public List<Role> Roles { get; set; }


    }

   
       
}