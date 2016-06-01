using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using TimeEffortCore.Entities;
using TimeEffort.Models;

namespace TimeEffort.Mappers
{
    public class RoleMapper
    {
        public static RoleViewModel MapRoleToModel(Role role)
        {
            return new RoleViewModel
            {
                Id = role.ID,
                Role = role.Name,
            };
         
        }
        public static Role MapRoleFromModel(RoleViewModel model)
        {
            return new Role
            {
                ID = model.Id,
                Name = model.Role
            };
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