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
                ProjectName=project.Name,
                CMoneyUsd=project.ContractUSD,
                CMoneyUzs=project.ContractUZS,
                PManagerId=project.ManagerID,
                ManagerName=project.UserInfo.FirstName+" "+project.UserInfo.LastName,
                StartDate=project.StartDate,
                FinishDate=project.EndDate,
                Status=(project.Status ==true )? "Active": "Inactive"
            };
         
        }
        public static Project MapProjectFromModel(ProjectViewModel model)
        {
            return new Project
            {
                ID = model.Id,
                Code = model.Code,
                Name = model.ProjectName,
                ContractUSD = model.CMoneyUsd,
                ContractUZS = model.CMoneyUzs,
                ManagerID = model.PManagerId,
                StartDate = model.StartDate.Date,
                EndDate = model.FinishDate.Date,
                Status = (model.Status=="Active") ? true:false
            };
        }
        public static List<ProjectViewModel> MapProjectsToModels(List<Project> list)
        {
                return list.Select(c => new ProjectViewModel
                {
                    Id = c.ID,
                    Code = c.Code,
                    ProjectName = c.Name,
                    CMoneyUsd = c.ContractUSD,
                    CMoneyUzs = c.ContractUZS,
                    PManagerId=c.ManagerID,
                    ManagerName = c.UserInfo.FirstName + " " + c.UserInfo.LastName,
                    StartDate = c.StartDate,
                    FinishDate = c.EndDate,
                    Status=(c.Status ==true )? "Active": "Inactive"
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