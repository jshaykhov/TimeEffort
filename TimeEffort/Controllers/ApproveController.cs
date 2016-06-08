using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Helper;
using TimeEffort.Mappers;
using TimeEffortCore.Services;

namespace TimeEffort.Controllers
{
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
           var workloads = WorkloadMapper.MapWorkloadsToModels(db.GetAll().Where(w => w.ApprovedPM == false && managedProjects.Contains((int)w.ProjectID)).ToList());
           return View("Index",  "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml",workloads);
        }
        [HttpGet]
        public object UpdateStatus(int id)
        {
            try
            {
                db.UpdateApproveStatus(id);
                return true;
            }
            catch
            {
                return false; 
            }
            
        }
        //
        // GET: /Approve/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Approve/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Approve/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Approve/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Approve/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Approve/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Approve/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
