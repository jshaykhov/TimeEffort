using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
namespace TimeEffort.Models
{
    public class AllUserModel
    {
        public IPagedList<ProjectViewModel> UserProjects { get; set; }
        public List<ProjectViewModel> PMProjects { get; set; }

    }

    //public class ProjectCreateViewModel
    //{
    //    [Required]
    //    [DataType(DataType.Text)]
    //    [Display(Name = "Project name")]
    //    public string ProjectName { get; set; }
    //    [Required]
    //    [Display(Name = "USD")]
    //    public decimal CMoneyUsd { get; set; }
    //    [Required]
    //    [Display(Name = "UZS")]
    //    public decimal CMoneyUzs { get; set; }

    //    [Required]
    //    [DataType(DataType.Date)]
    //    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    [Display(Name = "Start Date")]
    //    public DateTime StartDate { get; set; }
    //    [Required]
    //    [DataType(DataType.Date)]
    //    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    [Display(Name = "Finish Date")]
    //    public DateTime FinishDate { get; set; }


    //    //public List<CustomerViewModel> Customers { get; set; }
    //    //public List<UserViewModel> Users { get; set; }

    //}

    public class ProjectCreateViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }


       
        public String CType { get; set; }

       
        public string ProjectName { get; set; }
      
        public string CMoneyUsd { get; set; }
       
        public string CMoneyUzs { get; set; }

       
        public int PManagerId { get; set; }
       
        public int CustomerId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string Status { get; set; }
    }
    public class ProjectViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        [Display(Name = "Project code")]
        public string Code { get; set; }


        [Display(Name = "Type")]
        public String CType { get; set; }
     
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
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

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

        public string FullProjectName { get; set; }



        

       

        //public enum CodeType
       // {
        //    H = 0,
        //    M = 1,
        //    R = 2
        //}
    
}

    
}