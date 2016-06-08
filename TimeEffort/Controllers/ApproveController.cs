using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Helper;
using TimeEffort.Mappers;
using TimeEffort.Models;
using TimeEffortCore.Services;

namespace TimeEffort.Controllers
{
    [Authorize(Roles = "Admin, Master, Monitor, User,CTO, Test")]
    public class ApproveController : Controller
    {
        //---------------Singleton------------------
        public static WorkloadDbService _db;

        public static WorkloadDbService db
        {
            get
            {
                if (_db == null)
                    _db = new WorkloadDbService();
                return _db;
            }
        }
        //---------------Singleton------------------

        //
        // GET: /Approve/
        public ActionResult Index()
        {
           var managedProjects = HelperUser.GetProjectsByManager(User).Select(p=>p.ID).ToList();
            List<WorkloadViewModel> workloads;
            if (User.IsInRole("CTO"))
            {
                workloads = WorkloadMapper.MapWorkloadsToModels(db.GetAll().Where(w=> w.ApprovedPM == true).ToList());

            }
            else if (User.IsInRole("Master"))
            {
                workloads = WorkloadMapper.MapWorkloadsToModels(db.GetAll().Where(w => w.ApprovedCTO == true).ToList());
            }
            else 
            {
                workloads = WorkloadMapper.MapWorkloadsToModels(db.GetAll().Where(w => managedProjects.Contains((int)w.ProjectID)).ToList());
            }

           return View("Index",  "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml",workloads);
        }
        [HttpGet]
        public ActionResult UpdateStatus(int id)
        {
            
            try
            {
                if (User.IsInRole("CTO"))
                    db.UpdateApproveStatus(id, false, true, true);
                else if (User.IsInRole("Master"))
                    db.UpdateApproveStatus(id, true, true, true);
                else
                    db.UpdateApproveStatus(id, false, true, false);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }  
            
        }
    }
}
