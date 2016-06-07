using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using TimeEffort.Helper;
using TimeEffort.Mappers;
using TimeEffort.Models;
using TimeEffortCore.Entities;
using TimeEffortCore.Services;

namespace TimeEffort.Controllers
{
      [Authorize(Roles = "Admin, Master, CTO")]
    public class RoleController : Controller
    {
        //static properties does not requiere instantiation on accessing different Actions
        static RoleDBService _Service;
        //singleton to ensure that there is only one instance of the service

        private RoleDBService Service
        {
            get
            {
                if (_Service == null)
                    _Service = new RoleDBService();
                return _Service;
            }
        }
        // GET: /Role/
    
        public ActionResult Index()
        {
            var allRoles = Service.GetAll();
            var list = RoleMapper.MapRolesToModels(allRoles);
            return View("Index" , "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml",list);
        }

        // GET: Role/Create
        public ActionResult Create()
        {
            var model = new RoleViewModel();
            return View("Create" , "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml");
        }

        // POST: Role/Create
        [HttpPost]
        public ActionResult Create(RoleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var role = RoleMapper.MapRoleFromModel(model);

                    Service.Insert(role);
                    return RedirectToAction("Index");
                }

                return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }
        // GET: Role/Edit/5
        public ActionResult Edit(int id)
        {
            var model = RoleMapper.MapRoleToModel(Service.GetById(id));
            return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }

        // POST: Role/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, RoleViewModel model)
        {
            model.Id = id;
            try
            {
                if (ModelState.IsValid)
                {
                    var role = RoleMapper.MapRoleFromModel(model);
                    Service.Update(role);
                    return RedirectToAction("Index");
                }
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
            catch
            {
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }

        // GET: Role/Delete/5
        public ActionResult Delete(int id)
        {
            var role = Service.GetById(id);
            var model = RoleMapper.MapRoleToModel(role);
            return View("Delete" , "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }

        // POST: Role/Delete/5
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
                var role = Service.GetById(id);
                var model = RoleMapper.MapRoleToModel(role);
                ModelState.AddModelError("", e.Message);
                return View("Delete", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }
    }
}
