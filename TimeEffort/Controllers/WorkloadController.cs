using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Mappers;
using TimeEffort.Models;
using TimeEffortCore.Services;

namespace TimeEffort.Controllers
{
    [Authorize]
    public class WorkloadController : Controller
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


        // GET: /Workload/
        public ActionResult Index()
        {
            var username = this.HttpContext.User.Identity.Name;
            int userId = db.GetUserByUsername(username);
            var list = WorkloadMapper.MapWorkloadsToModels(db.GetAll());
            list = list.Where(u => u.UserId == userId).ToList();
            GetWorkloadDuration(list);
            return View();
        }
        private Dictionary<DateTime, Decimal> GetWorkloadDuration(List<WorkloadViewModel> list)
        {
            int length = list.Select(u => u.Date).Distinct().Count();
            List < DateTime >dates= list.Select(u => u.Date).Distinct().ToList();
            Dictionary<DateTime, Decimal> result = new Dictionary<DateTime, Decimal>();
            List<WorkloadViewModel> listOf = (from l in list group l by l.Date into g select new WorkloadViewModel { Date=g.First().Date, Duration=g.Sum(d=>d.Duration)}).ToList();
            foreach (var a in listOf)
            {
                result.Add(a.Date, a.Duration);
            }
            return result;
        }
        [HttpGet]
        public ActionResult Create(string dateClicked)
        {
            var model = new WorkloadViewModel();
            model.Date = DateTime.Parse(dateClicked).Date;
            CreateSelectListForDropDownWlTypes();
            CreateSelectListForDropDownProjects();
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
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Service/Edit/5
        public ActionResult Edit(int id)
        {
            var model = WorkloadMapper.MapWorkloadToModel(db.GetById(id));
            return View(model);
        }

        // POST: Service/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, WorkloadViewModel model)
        {
            model.Id = id;
            try
            {
                if (ModelState.IsValid)
                {
                    var service = WorkloadMapper.MapWorkloadFromModel(model);
                    db.Update(service);
                    return RedirectToAction("Index");
                }
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

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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