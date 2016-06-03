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
                PositionId=c.PositionID,
                UserName=c.Username              
               
            }).ToList();
        }

           }
}