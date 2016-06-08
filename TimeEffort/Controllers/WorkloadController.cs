using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Helper;
using TimeEffort.Mappers;
using TimeEffort.Models;
using TimeEffortCore.Entities;
using TimeEffortCore.Services;

namespace TimeEffort.Controllers
{
    [Authorize]
    public class WorkloadController : Controller
    {
        //---------------Singleton------------------
        public static WorkloadDbService _db;
        private static DateTime date;
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

        [HttpGet]
        public ActionResult GetWorkload()
        {
            var username = this.HttpContext.User.Identity.Name;
            int userId = db.GetUserByUsername(username);
            var list = WorkloadMapper.MapWorkloadsToModels(db.GetAll());
            list = list.Where(u => u.UserId == userId).ToList();
            object[] arr = GetWorkloadDuration(list);
            return Json(arr, JsonRequestBehavior.AllowGet);
        }
        private object[] GetWorkloadDuration(List<WorkloadViewModel> list)
        {
            int length = list.Select(u => u.Date).Distinct().Count();
            List<DateTime> dates = list.Select(u => u.Date).Distinct().ToList();
            List<WorkloadViewModel> listOf = (from l in list group l by l.Date into g select new WorkloadViewModel { Date = g.First().Date, Duration = g.Sum(d => d.Duration) }).ToList();
            object[] arr = new object[listOf.Count];
            for (int i = 0; i < listOf.Count; i++)
            {
                arr[i] = new { start = listOf.ElementAt(i).Date.GetDateTimeFormats()[4], title = listOf.ElementAt(i).Duration };
            }
            return arr;
        }
        // GET: /Workload/
        public ActionResult Index()
        {  
            return View("Index", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml");
        }
        public ActionResult Workloads(string dateClicked)
        {
            var username = this.HttpContext.User.Identity.Name;
            int userId = db.GetUserByUsername(username);
            DateTime workloadDate;
            if (!String.IsNullOrEmpty(dateClicked))
            {
                workloadDate = DateTime.Parse(dateClicked);
                date = workloadDate;
            }
            else
                workloadDate = date;
            var list = WorkloadMapper.MapWorkloadsToModels(db.GetAllbyUserAndDate(userId,workloadDate));
            return View(list);
        }
        [HttpGet]
        public DateTime GetDate()
        {
            return date;
        }
        [HttpGet]
        public ActionResult Create(DateTime dateClicked)
        {
            var model = new WorkloadCreateModel();
            model.Types = db.GetAllTypes();
            model.Projects = db.GetAllProjects();
            if (User.IsInRole("User")) { 
                model.Projects = HelperUser.GetAllInvolvedProjects(User);
            }
            model.Date = dateClicked;
            //var model = new WorkloadViewModel();
            //model.Date = dateClicked;
            //CreateSelectListForDropDownWlTypes();
            //CreateSelectListForDropDownProjects();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(WorkloadViewModel model)
        {
            var username = this.HttpContext.User.Identity.Name;
            model.UserId = db.GetUserByUsername(username);
            model.Approved = false;
            
            if (ModelState.IsValid)
            {
                var workload = WorkloadMapper.MapWorkloadFromModel(model);
                db.Insert(workload);
                return RedirectToAction("Workloads");
            }
            return View(model);
        }

        // GET: Service/Edit/5
        public ActionResult Edit(int id)
        {
            CreateSelectListForDropDownWlTypes();
            CreateSelectListForDropDownProjects();
            var model = WorkloadMapper.MapWorkloadToModel(db.GetById(id));
            return View(model);
        }

        // POST: Service/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, WorkloadViewModel model)
        {
            model.Id = id;
            var username = this.HttpContext.User.Identity.Name;
            model.UserId = db.GetUserByUsername(username);
            try
            {
                if (ModelState.IsValid)
                {
                    var service = WorkloadMapper.MapWorkloadFromModel(model);
                    db.Update(service);
                    return RedirectToAction("Workloads");
                }
                CreateSelectListForDropDownWlTypes();
                CreateSelectListForDropDownProjects();
                return View(model);

            }
            catch
            {
                return View();
            }
        }

        // GET: Service/Delete/5
        public ActionResult Delete(int id)
        {
            var model = WorkloadMapper.MapWorkloadToModel(db.GetById(id));
            return View(model);
        }

        // POST: Service/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                db.Delete(id);

                return RedirectToAction("Workloads");
            }
            catch
            {
                CreateSelectListForDropDownWlTypes();
                CreateSelectListForDropDownProjects();
                return View();
            }
        }

        [HttpGet]
        public ActionResult ReceiveList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReceiveList(List<RequestDataJson> query)
        {
            var userId = db.GetUserByUsername(User.Identity.Name);
            foreach (RequestDataJson item in query)
            {
                if (item != null) { 
                    var tempWorkload = new Workload
                    {
                        ApprovedCTO = false,
                        ApprovedMaster = false,
                        ApprovedPM = false,
                        Date = item.Date,
                        Duration = 4,
                        ProjectID = item.ProjectId.GetValueOrDefault(),
                        UserID = userId,
                        WorkloadTypeID = item.TypeId,
                        Note = item.Notes
                    };
                    db.Insert(tempWorkload);
                }
            }

            return Json(new { response = "Successfully completed" }, JsonRequestBehavior.DenyGet);
        }


        private void CreateSelectListForDropDownWlTypes()
        {
            
            var list = db.GetAllTypes();
            SelectList types = new SelectList(WloadTypeMapper.MapWorkloadTypesToModels(list),
                                                   "Id ", "WloadType");
            //store list of types in ViewBag 
            //for further use in view's dropdown list
            ViewBag.Types = types;
        }
        private void CreateSelectListForDropDownProjects()
        {

            var list = db.GetAllProjects();
            SelectList projects = new SelectList(ProjectMapper.MapProjectsToModels(list),
                                                   "Id ", "FullProjectName");
            //store list of projects in ViewBag 
            //for further use in view's dropdown list
            ViewBag.Projects = projects;
        }
	}
}