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
    [Authorize(Roles = "Admin, Master, Monitor, User,CTO, Test")]
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
            var list = WorkloadMapper.MapWorkloadsToModels(db.GetAllbyUserAndDate(userId, workloadDate));
            return View("Workloads", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", list);
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
            if (User.IsInRole("User"))
            {
                model.Projects = HelperUser.GetAllInvolvedProjects(User);
            }
            model.Date = dateClicked;

            int userId = db.GetUserByUsername(User.Identity.Name);

            model.Workloads = WorkloadMapper.MapWorkloadsToModels(db.GetAllbyUserAndDate(userId, dateClicked));

            model.Total = 0;

            foreach (var item in model.Workloads)
                model.Total += item.Duration;

            return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(WorkloadViewModel model)
        {
            var username = this.HttpContext.User.Identity.Name;
            model.UserId = db.GetUserByUsername(username);


            if (ModelState.IsValid)
            {
                var workload = WorkloadMapper.MapWorkloadFromModel(model);
                db.Insert(workload);
                return RedirectToAction("Workloads");
            }
            return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }

        // GET: Service/Edit/5
        public ActionResult Edit(int id)
        {
            CreateSelectListForDropDownWlTypes();
            CreateSelectListForDropDownProjects();
            var model = WorkloadMapper.MapWorkloadToModel(db.GetById(id));
            return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
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
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);

            }
            catch
            {
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml");
            }
        }

        // GET: Service/Delete/5
        public ActionResult Delete(int id)
        {
            var model = WorkloadMapper.MapWorkloadToModel(db.GetById(id));
            return View("Delete", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
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
                return View("Delete", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml");
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
                if (item != null)
                {
                    var tempWorkload = new Workload
                    {
                        ApprovedCTO = false,
                        ApprovedMaster = false,
                        ApprovedPM = false,
                        Date = item.Date,
                        Duration = decimal.Parse(item.Duration),
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

        private DateTime GetMonday(DateTime today)
        {
            var startDate = today;
            var endDate = startDate.AddDays(7);
            //the number of days in our range of dates
            var numDays = (int)((endDate - startDate).TotalDays);
            List<DateTime> myDates = Enumerable
                //creates an IEnumerable of ints from 0 to numDays
                       .Range(0, numDays)
                //now for each of those numbers (0..numDays), 
                //select startDate plus x number of days
                       .Select(x => startDate.AddDays(x))
                //and make a list
                       .ToList();
            List<string> myDaysOfWeek = myDates.Select(d => d.DayOfWeek.ToString()).ToList();
            startDate = startDate.AddDays(-(((startDate.DayOfWeek - DayOfWeek.Monday) + 7) % 7));

            return startDate;
        }

        private Dictionary<Project, List<CalendarDetails>> GetCalendarResults(DateTime? today)
        {
            var list = HelperUser.GetProjectsByWorkingUser(User);
            list.AddRange(HelperUser.GetProjectsByManager(User));
            DateTime Monday;
            if (today.HasValue)
            {
                DateTime StartDate = today.GetValueOrDefault();
                Monday = GetMonday(StartDate);
            }
            else
                Monday = GetMonday(DateTime.Today);

            var Sunday = Monday.AddDays(7);

            var workloads = db.GetWorkloadsByUser(User.Identity.Name).FindAll(x => x.Date >= Monday && x.Date <= Sunday);

            Dictionary<Project, List<CalendarDetails>> viewModel = new Dictionary<Project, List<CalendarDetails>>();
            Project overhead = new Project(); overhead.Name = "Overhead";
            var overHeadList = new List<CalendarDetails>();

            foreach (var project in list)
            {
                var tempList = new List<CalendarDetails>();
                foreach (var workload in workloads)
                {
                    if (workload.ProjectID.HasValue)
                    {
                        if (project.ID == workload.ProjectID)
                            tempList.Add(new CalendarDetails
                            {
                                Date = workload.Date,
                                Duration = workload.Duration,
                                Type = workload.WorkloadType
                            });
                    }
                    else
                    {
                        overHeadList.Add(new CalendarDetails
                            {
                                Date = workload.Date,
                                Duration = workload.Duration,
                                Type = workload.WorkloadType
                            });
                    }
                }

                viewModel.Add(project, tempList);
            }

            viewModel.Add(overhead, overHeadList);

            return viewModel;

        }

        public ActionResult Calendar()
        {
            //return View(GetCalendarResults(today));

            var model = new CalendarReturningModel();
            model.Monday = GetMonday(DateTime.Today);
            model.Workloads = db.GetWorkloadsByUser(User.Identity.Name).FindAll(x => x.Date >= model.Monday && x.Date <= model.Monday.AddDays(7));
            return View(model);

        }

        [HttpPost]
        public ActionResult Calendar(DateTime? today)
        {
            return View(GetCalendarResults(today));
        }
    }
}