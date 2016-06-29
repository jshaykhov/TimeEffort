using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using TimeEffortCore.Entities;
using TimeEffort.Models;

namespace TimeEffort.Mappers
{
    public class ProjectMapper
    {
        public static ProjectViewModel MapProjectToModel(Project project)
        {
            return new ProjectViewModel
            {
                Id = project.ID,
                Code = project.Code,
                //CType=project.CType == null ? "" : project.CType,
                ProjectName=project.Name,
                CMoneyUsd=project.ContractUSD,
                CMoneyUzs=project.ContractUZS,
                PManagerId=project.ManagerID,
                ManagerName=project.UserInfo.FirstName+" "+project.UserInfo.LastName,
                Customer=project.Customer.Name,
                StartDate=project.StartDate,
                FinishDate=project.EndDate,
                Status=project.Status
            };
         
        }
        public static Project MapProjectFromCreateModel(ProjectCreateViewModel model)
        {
            return new Project
            {
                ID = model.Id,
                CType = model.CType,
                Code = model.Code,
                Name = model.ProjectName,
                ContractUSD = decimal.Parse(model.CMoneyUsd),
                ContractUZS = decimal.Parse(model.CMoneyUzs),
                ManagerID = model.PManagerId,
                CustomerId = model.CustomerId,
                StartDate = model.StartDate.Date,
                EndDate = model.FinishDate.Date,
                Status = model.Status
            };
        }
        public static Project MapProjectFromModel(ProjectViewModel model)
        {
            return new Project
            {
                ID = model.Id,
                CType = model.CType == null ? "" : model.CType,
                Code = model.Code,
                Name = model.ProjectName,
                ContractUSD = model.CMoneyUsd,
                ContractUZS = model.CMoneyUzs,
                ManagerID = model.PManagerId,
                CustomerId = model.CustomerId,
                StartDate = model.StartDate.Date,
                EndDate = model.FinishDate.Date,
                Status = model.Status
            };
        }
        public static List<ProjectViewModel> MapProjectsToModels(List<Project> list)
        {
                return list.Select(c => new ProjectViewModel
                {
                    Id = c.ID,
                    Code = c.Code,
                    CType = c.CType == null ? "" : c.CType,
                    ProjectName = c.Name,
                    CMoneyUsd = c.ContractUSD,
                    CMoneyUzs = c.ContractUZS,
                    PManagerId=c.ManagerID,
                    Customer=c.Customer.Name,
                    ManagerName = c.UserInfo.FirstName + " " + c.UserInfo.LastName,
                    StartDate = c.StartDate,
                    FinishDate = c.EndDate,
                    Status=c.Status,
                    FullProjectName=c.Code+" "+c.Name
                }).ToList();
        }

        //users
        public static List<UserViewModel> MapUsersToModels(List<UserInfo> list)
        {
            return list.Select(u => new UserViewModel
            {
                Id = u.ID,
                FullName=u.FirstName + " "+u.LastName
            }).ToList();
        }
        
        
    }
}