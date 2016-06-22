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
      [Authorize(Roles = "Admin")]
    public class WorkloadTypeController : Controller
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
        // GET: /WloadType/
        public ActionResult Index()
        {
            var allWloadTypes = Service.GetAllWorkloadTypes();
            var list = WloadTypeMapper.MapWorkloadTypesToModels(allWloadTypes);
            return View("Index", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", list);
        }

        // GET: WloadType/Create
        public ActionResult Create()
        {
            var model = new WloadTypeViewModel();
            return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml");
        }

        // POST: WloadType/Create
        [HttpPost]
        public ActionResult Create(WloadTypeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var wloadtype = WloadTypeMapper.MapWorkloadTypeFromModel(model);

                    Service.Insert(wloadtype);
                    return RedirectToAction("Index");
                }

                return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml",model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }
        // GET: WloadType/Edit/5
        public ActionResult Edit(int id)
        {
            var model = WloadTypeMapper.MapWorkloadTypeToModel(Service.GetWorkloadTypeById(id));
            return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }

        // POST: WloadType/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, WloadTypeViewModel model)
        {
            model.Id = id;
            try
            {
                if (ModelState.IsValid)
                {
                    var wloadtype = WloadTypeMapper.MapWorkloadTypeFromModel(model);
                    Service.Update(wloadtype);
                    return RedirectToAction("Index");
                }
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
            catch
            {
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }

        // GET: WloadType/Delete/5
        public ActionResult Delete(int id)
        {
            var wloadtype = Service.GetWorkloadTypeById(id);
            var model = WloadTypeMapper.MapWorkloadTypeToModel(wloadtype);
            return View("Delete", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }

        // POST: WloadType/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Service.DeleteWorkloadType(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var wloadtype = Service.GetWorkloadTypeById(id);
                var model = WloadTypeMapper.MapWorkloadTypeToModel(wloadtype);
                ModelState.AddModelError("", e.Message);
                return View("Delete", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }
    }
    }