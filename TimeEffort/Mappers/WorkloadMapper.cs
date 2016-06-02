using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeEffort.Models;
using TimeEffortCore.Entities;

namespace TimeEffort.Mappers
{
    public class WorkloadMapper
    {
        //mapping workload to the View Model
        public static WorkloadViewModel MapWorkloadToModel(Workload item)
        {
            return new WorkloadViewModel
            {
                Id = item.ID,
                Date = item.Date,
                Project = item.Project,                     //pay attention
                User = item.UserInfo,                       //pay attention
                Approved = item.Approved,
                Duration = item.Duration,
                Note = item.Note,
                WorkLoadType = item.WorkloadType             //pay attention
            };
        }


        //mapping view model to the Workload

        public static Workload MapWorkloadFromModel(WorkloadViewModel model)
        {
            return new Workload
            {
                ID = model.Id,
                Date = model.Date,
                UserID = model.User.ID,
                ProjectID = model.Project.ID,
                Duration = model.Duration,
                Approved = model.Approved,
                Note = model.Note,
                WorkloadTypeID = model.WorkLoadType.ID,                
            };
        }



        public static List<WorkloadViewModel> MapWorkloadsToModels(List<Workload> list)
        {
            return list.Select(item => new WorkloadViewModel
            {
                Id = item.ID,
                Date = item.Date,
                Project = item.Project,                     //pay attention
                User = item.UserInfo,                       //pay attention
                Approved = item.Approved,
                Note = item.Note,
                Duration = item.Duration,
                WorkLoadType = item.WorkloadType             //pay attention
            }).ToList();
        }

        public static List<Workload> MapWorkloadsFromModels(List<WorkloadViewModel> list)
        {
            return list.Select(item => new Workload
            {
                ID = item.Id,
                UserID = item.User.ID,
                ProjectID = item.Project.ID,
                Duration = item.Duration,
                Approved = item.Approved,
                Note = item.Note,
                WorkloadTypeID = item.WorkLoadType.ID,
            }).ToList();
        }
    }
}