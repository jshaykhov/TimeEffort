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
    public class AppointController : Controller
    {
        //static properties does not requiere instantiation on accessing different Actions
        static AccessDBService _Service;
       
        //static UserService UserService;
        //singleton to ensure that there is only one instance of the service

        private AccessDBService Service
        {
            get
            {
                if (_Service == null)
                    _Service = new AccessDBService();
                return _Service;
            }
        }


        //
        // GET: /Appoint/
        public ActionResult Index()
        {
            var allAppoints = Service.GetAll();
            var list = AppointMapper.MapAppointsToModels(allAppoints);
            return View("Index", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", list);
           
        }

        
        //
        // GET: /Appoint/Create
        public ActionResult Create()
        {
            var model = new AppointViewModel();
            CreateSelectListForUsers();
            CreateSelectListForDropDownProjects();
            CreateSelectListForDropDownRoles();
            return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }

        //
        // POST: /Appoint/Create
        [HttpPost]
        public ActionResult Create(AppointViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                      var appoint = AppointMapper.MapAppointFromModel(model);
                     _Service.Insert(appoint);
                    return RedirectToAction("Index");
                }
                CreateSelectListForUsers();
                CreateSelectListForDropDownProjects();
                CreateSelectListForDropDownRoles();
                return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml");


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml");
            }
        }





        //
        // GET: /Appoint/Edit/5
        public ActionResult Edit(int id)
        {
            CreateSelectListForUsers();
            CreateSelectListForDropDownProjects();
            CreateSelectListForDropDownRoles();
            var model = AppointMapper.MapAppointToModel(Service.GetById(id));
            return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }

        //
        // POST: /Appoint/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, AppointViewModel model)
        {
          
            model.Id = id;
            try
            {
                if (ModelState.IsValid)
                {
                    var appoint = AppointMapper.MapAppointFromModel(model);
                    Service.Update(appoint);
                    return RedirectToAction("Index");
                }
                CreateSelectListForUsers();
                CreateSelectListForDropDownProjects();
                CreateSelectListForDropDownRoles();
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
            catch
            {
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }

        //
        // GET: /Appoint/Delete/5
        public ActionResult Delete(int id)
        {
            var appoint = Service.GetById(id);
            var model = AppointMapper.MapAppointToModel(appoint);
            return View("Delete", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }

        //
        // POST: /Appoint/Delete/5
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
                var appoint = Service.GetById(id);
                var model = AppointMapper.MapAppointToModel(appoint);
                ModelState.AddModelError("", e.Message);
                return View("Delete", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }
        //DROPDOWN for PROJECT ROLE USER
        private void CreateSelectListForDropDownProjects()
        {
            var model = new AppointViewModel();
            int id = model.ProjectID;
            var list = _Service.GetAllProjects();
            SelectList projects = new SelectList(AppointMapper.MapProjectsToModels(list),
                                                   "Id", "ProjectName");
            //store list of projects in ViewBag 
            //for further use in view's dropdown list
            ViewBag.Projects = projects;
        }

        private void CreateSelectListForDropDownRoles()
        {
            var model = new AppointViewModel();
            int id = model.RoleID;
            var list = _Service.GetAllRoles();
            SelectList roles = new SelectList(AppointMapper.MapRolesToModels(list),
                                                    "Id", "Role");
            //store list of projects in ViewBag 
            //for further use in view's dropdown list
            ViewBag.Roles = roles;
        }

        private void CreateSelectListForUsers()
        {
            var model = new AppointViewModel();
            int id = model.UserID;
            var list = _Service.GetAllUsers();
            SelectList users = new SelectList(AppointMapper.MapUsersToModels(list),
                                                    "Id", "FullName", id);
            //store list of projects in ViewBag 
            //for further use in view's dropdown list
            ViewBag.Users = users;
        }
    }
}
