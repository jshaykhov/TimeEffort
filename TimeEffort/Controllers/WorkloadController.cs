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
            //var list = WorkloadMapper.MapWorkloadsToModels(db.GetAll());
            return View();
        }

        public ActionResult Create(string dateClicked)
        {
            var model = new WorkloadViewModel();
            //model.Date = DateTime.Parse(dateClicked);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(WorkloadViewModel model)
        {
            /*var username = this.HttpContext.User.Identity.Name;
            var user = db.GetByName(username);
            model.User = user;*/

            //model.User = db.GetByName(this.HttpContext.User.Identity.Name); //Get current user via his username

            
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
            //store list of users in ViewBag 
            //for further use in view's dropdown list
            ViewBag.Types = types;
        }
        public ActionResult Monitor()
        {
            var allWorkloads = WorkloadMapper.MapWorkloadsToModels(db.GetAll());
             return View(allWorkloads);
            
        }

	}
}