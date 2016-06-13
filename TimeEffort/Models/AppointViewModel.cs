using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TimeEffortCore.Entities;

namespace TimeEffort.Models
{
    public class AppointCreateModel
    {
        public List<UserViewModel> Emps { get; set; }
        public List<RoleViewModel> Roles { get; set; }
        public string Message { get; set; }
        //public List<ProjectViewModel> ProjectList { get; set; }
    }

    public class AppointListModel {
        public List<ProjectViewModel> Projects { get; set; }
        public int selectedProject { get; set; }
    }
    public class AppointViewModel
    {

        public int TotalPages { get; set; }
        public int Id { get; set; }

        [Display(Name = "User")]     
        public int UserID { get; set; }

       
        [DataType(DataType.Text)]
        [Display(Name = "Employee")]
        public string User { get; set; }


        [Display(Name = "Project")]
        public int ProjectID { get; set; }

       
         [DataType(DataType.Text)]
         [Display(Name = "Project name")]
        public string Project { get; set; }

         [Display(Name = "Role ")]       
        public int RoleID { get; set; }

       
        [DataType(DataType.Text)]
        [Display(Name = "Role")]
        public string Role { get; set; }

        
    }

   

   
       
}