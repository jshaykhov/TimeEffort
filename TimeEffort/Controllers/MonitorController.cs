using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Helper;
using TimeEffort.Models;
using TimeEffortCore.Services;
using System.IO;
using System.Data;
using ClosedXML.Excel;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace TimeEffort.Controllers
{
    [Authorize(Roles = "Admin, Master, Monitor, User,CTO, Test")]
    public class MonitorController : Controller
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
        // GET: /Monitor/
        public ActionResult Index()
        {
            var model = new MonitorViewModel();
            model.allEmployees = HelperUser.GetAllUsers();
            model.allProjects = db.GetAllProjects();
            model.workloads = HelperUser.GetAllWorkloadTypes();
            return View("Index", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml",model);
        }

        [HttpGet]
        public ActionResult MyAjaxRequest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MyAjaxRequest(QueryJson myQuery)
        {
            DateTime from = DateTime.Now.AddDays(-7), to=DateTime.Now;
            if (DateTime.TryParse(myQuery.FromDate, out from))
                from = from;
            if (DateTime.TryParse(myQuery.ToDate, out to))
                to = to;


            MonitorViewModel model = new MonitorViewModel
            {
                query = new QueryMonitor
                {
                    Employee = myQuery.SelectedUser,
                    Project = myQuery.SelectedProject,
                    WorkloadType = myQuery.SelectedType,
                    FromDate = from,
                    ToDate = to
                }
            };
            model = GetResult(model);
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        public MonitorViewModel GetResult(MonitorViewModel model)
        {

            model.projects = GetProjects(DateTime.Now, model.query.FromDate, model.query.ToDate, model.query.Employee, model.query.Project, model.query.WorkloadType);
            
            return model;
        }

        private List<ProjectMontior> GetProjects(DateTime now, DateTime? from = null, DateTime? to = null, string user = "all", string project = "all", string type = "all")
        {
            if (from == null)
                from = now.AddDays(-7);
            if (to == null)
                to = now;


            var allWorkloads = db.GetAll().FindAll(x => x.Date >= from && x.Date <= to).ToList();
            if (!user.Equals("All"))
            {
                var tempUser = HelperUser.GetUserByName(user);
                allWorkloads = allWorkloads.FindAll(x => x.UserInfo.Username == user).ToList();
            }

            if (!project.Equals("All"))
            {
                var tempProject = HelperUser.GetProjectByCode(project);
                allWorkloads = allWorkloads.FindAll(x => x.ProjectID == tempProject.ID).ToList();
            }
            if (!type.Equals("All"))
            {
                var tempWorkload = HelperUser.GetAllWorkloadTypes().FirstOrDefault(w=>w.Name == type);
                allWorkloads = allWorkloads.FindAll(x => x.WorkloadTypeID == tempWorkload.ID).ToList();
            }

            return allWorkloads.Select(c => new ProjectMontior
            {
                Date = c.Date.ToString(),
                Duration = c.Duration.ToString(),
                Employee = c.UserInfo.FirstName + " " + c.UserInfo.LastName,
                Project = c.Project == null ? "Overhead" : c.Project.Code,
                Type = c.WorkloadType.Name,
                Id = 0
            }).ToList();
        }

        

       
	}
}