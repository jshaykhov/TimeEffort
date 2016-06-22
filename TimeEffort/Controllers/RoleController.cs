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
        static AllDBServices _Service;
        //singleton to ensure that there is only one instance of the service

        private AllDBServices Service
        {
            get
            {
                if (_Service == null)
                    _Service = new AllDBServices();
                return _Service;
            }
        }
        // GET: /Role/
    
        public ActionResult Index()
        {
            var allRoles = Service.GetAllRoles();
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
            var model = RoleMapper.MapRoleToModel(Service.GetRoleById(id));
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
            var role = Service.GetRoleById(id);
            var model = RoleMapper.MapRoleToModel(role);
            return View("Delete" , "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }

        // POST: Role/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Service.DeleteRole(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var role = Service.GetRoleById(id);
                var model = RoleMapper.MapRoleToModel(role);
                ModelState.AddModelError("", "This role is currently involved in one or more users. Deleting failed. "+"\n" + e.Message);
                return View("Delete", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }
    }
}
