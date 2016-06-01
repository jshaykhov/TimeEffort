using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Mappers;
using TimeEffortCore.Services;

namespace TimeEffort.Controllers
{
    public class ProjectController : Controller
    {
        //static properties does not requiere instantiation on accessing different Actions
        static ProjectDBService _Service;
        //singleton to ensure that there is only one instance of the service
        private ProjectDBService Service
        {
            get
            {
                if (_Service == null)
                    _Service = new ProjectDBService();
                return _Service;
            }
        }

        //
        // GET: /Project/
        public ActionResult Index()
        {
            var allProjects = Service.GetAll();
            var list = ProjectMapper.MapProjectsToModels(allProjects);
            return View(list);

        }

        //
        // GET: /Project/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Project/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Project/Create
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
        // GET: /Project/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Project/Edit/5
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
        // GET: /Project/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Project/Delete/5
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
