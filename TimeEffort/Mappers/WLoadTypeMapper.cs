using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using TimeEffortCore.Entities;
using TimeEffort.Models;

namespace Shop.Web.Mappers
{
    public class WloadTypeMapper
    {
        public static WloadTypeViewModel MapWorkloadTypeToModel(WorkloadType type)
        {
            return new WloadTypeViewModel
            {
                Id = type.ID,
                WloadType = type.Name
            };
         
        }
        public static WorkloadType MapWorkloadTypeFromModel(WloadTypeViewModel model)
        {
            return new WorkloadType
            {
                ID = model.Id,
                Name=model.WloadType
            };
        }
        public static List<WloadTypeViewModel> MapWorkloadTypesToModels(List<WorkloadType> list)
        {
                return list.Select(c => new WloadTypeViewModel
                {
                    Id = c.ID,
                    WloadType=c.Name
                }).ToList();
        }
    }
}