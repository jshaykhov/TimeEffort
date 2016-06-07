using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeEffort.Models;
using TimeEffortCore.Entities;

namespace TimeEffort.Mappers
{
    public class UserMapper
    {
        public static UserViewModel MapUserToModel(UserInfo userInfo)
        {
            return new UserViewModel
            {
                Id = userInfo.ID,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                Phone = userInfo.Phone,
                Address=userInfo.Address,
                Major=userInfo.Major,
                Email = userInfo.Email,
                Position = userInfo.Position.Name,
                PositionId=userInfo.PositionID,
                UserName = userInfo.Username,
                Password=userInfo.Password
            };

        }
        public static UserInfo MapUserFromModel(UserViewModel model)
        {
            return new UserInfo
            {
                ID = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                Address = model.Address,
                Major = model.Major,
                Email = model.Email,
                PositionID=model.PositionId,
                //Position = model.Position,
                Username=model.UserName
            };
        }
            public static List<UserViewModel> MapUsersToModels(List<UserInfo> list)
        {
            return list.Select(c => new UserViewModel
            {
                Id = c.ID,
                FirstName = c.FirstName,
                LastName=c.LastName,
                Phone=c.Phone,
                Email=c.Email,
                Address = c.Address,
                Major = c.Major,
                Position=c.Position.Name,
                PositionId=c.PositionID,
                UserName=c.Username              
               
            }).ToList();
        }

           

           

           }




}