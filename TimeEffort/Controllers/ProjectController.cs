﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Mappers;
using TimeEffort.Models;
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
            CreateSelectListForDropDown();
            var model = new ProjectViewModel();
            return View(model);

        }

        //
        // POST: /Project/Create
        [HttpPost]
        public ActionResult Create(ProjectViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var project = ProjectMapper.MapProjectFromModel(model);

                    Service.Insert(project);
                    return RedirectToAction("Index");
                }
                CreateSelectListForDropDown();
                return View(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }

        }

        //
        // GET: /Project/Edit/5
        public ActionResult Edit(int id)
        {
            var model = ProjectMapper.MapProjectToModel(Service.GetById(id));
            return View(model);
        }

        //
        // POST: /Project/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ProjectViewModel model)
        {
            model.Id = id;
            try
            {
                if (ModelState.IsValid)
                {
                    var project = ProjectMapper.MapProjectFromModel(model);
                    Service.Update(project);
                    return RedirectToAction("Index");
                }
                CreateSelectListForDropDown();
                return View(model);
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
            var project = Service.GetById(id);
            var model = ProjectMapper.MapProjectToModel(project);
            return View(model);
        }

        //
        // POST: /Project/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Service.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var project = Service.GetById(id);
                var model = ProjectMapper.MapProjectToModel(project);
                ModelState.AddModelError("", e.Message);
                return View(model);
            }

        }
        private void CreateSelectListForDropDown()
        {
            //Create selectlist of users 
            //to pass it to view's dropdown
            //to allow user selection during project update/create
            var listOfUsers = Service.GetAllUsers();
            SelectList users = new SelectList(ProjectMapper.MapUsersToModels(listOfUsers),
                                                   "Id ", "FullName");
            //store list of users in ViewBag 
            //for further use in view's dropdown list
            ViewBag.Users = users;
        }

    }
}
