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
            public static List<UserViewModel> MapUsersToModels(List<UserInfo> list)
        {
            return list.Select(c => new UserViewModel
            {
                Id = c.ID,
                FirstName = c.FirstName,
                LastName=c.LastName,
                Phone=c.Phone,
                Email=c.Email,
                Position=c.Position.Name,
                UserName=c.Username              
               
            }).ToList();
        }

            public static UserViewModel MapUserToModel(UserInfo userInfo)
            {
                return new UserViewModel
                {
                    Id =userInfo.ID,
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    Phone = userInfo.Phone,
                    Email = userInfo.Email,
                    Position = userInfo.Position.Name,
                    UserName = userInfo.Username  
                };

            }



           }




}