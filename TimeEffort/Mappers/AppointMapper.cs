using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeEffort.Models;

namespace TimeEffort.Mappers
{
    public class AppointMapper
    {
        public static AppointViewModel MapAppointToModel(Access access)
        {
            return new AppointViewModel
            {
             Id=access.ID,
             UserID=access.UserID,
             User=access.UserInfo.FirstName+" "+access.UserInfo.LastName,
             ProjectID=access.ProjectID,
             Project=access.Project.Name,
             RoleID=access.RoleID,
             Role = access.Role.Name,
             DateFrom=access.DateFrom,
             DateTo=access.DateTo
           
            };

        }
        public static Access MapAppointFromModel(AppointViewModel model)
        {
            return new Access
            {
                ID=model.Id,
                UserID = model.UserID,
                ProjectID = model.ProjectID,
                RoleID = model.RoleID,
                DateFrom=model.DateFrom,
                DateTo=model.DateTo
            };
        }
        public static List<AppointViewModel> MapAppointsToModels(List<Access> list)
        {
            return list.Select(c => new AppointViewModel
            {
                Id = c.ID,
                UserID = c.UserID,
                User = c.UserInfo.FirstName + " " + c.UserInfo.LastName,
                ProjectID = c.ProjectID,
                Project = c.Project.Name,
                ProjectFullName=c.Project.Code+" "+c.Project.Name,
                RoleID = c.RoleID,
                Role = c.Role.Name,
                DateFrom=c.DateFrom,
                DateTo=c.DateTo
            }).ToList();
        }
        public static List<UserViewModel> MapUsersToModels(List<UserInfo> list)
        {
            return list.Select(u => new UserViewModel
            {
                Id = u.ID,
                FullName = u.FirstName + " " + u.LastName
            }).ToList();
        }

        public static List<ProjectViewModel> MapProjectsToModels(List<Project> list)
        {
            return list.Select(u => new ProjectViewModel
            {
                Id = u.ID,
                ProjectName = u.Name
            }).ToList();
        }
        public static List<RoleViewModel> MapRolesToModels(List<Role> list)
        {
            return list.Select(c => new RoleViewModel
            {
                Id = c.ID,
                Role = c.Name
            }).ToList();
        }
    }
}