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
    [Authorize(Roles = "Admin, Master, Monitor,CTO, Test")]
    public class CustomerController : Controller
    {
        static CustomerDBService _Service;
        private CustomerDBService Service
        {
            get
            {
                if (_Service == null)
                    _Service = new CustomerDBService();
                return _Service;
            }
        }
        //
        // GET: /Customer/
        public ActionResult Index()
        {
            var allRoles = Service.GetAll();
            var list = CustomerMapper.MapCustomersToModels(allRoles);
            return View("Index", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", list);
        }

        //
        // GET: /Customer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Customer/Create
        public ActionResult Create()
        {
            var model = new CustomerViewModel();
            return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml");
        }

        //
        // POST: /Customer/Create
        [HttpPost]
        public ActionResult Create(CustomerViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var customer = CustomerMapper.MapCustomerFromModel(model);

                    Service.Insert(customer);
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

        //
        // GET: /Customer/Edit/5
        public ActionResult Edit(int id)
        {
            var model = CustomerMapper.MapCustomerToModel(Service.GetById(id));
            return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }

        //
        // POST: /Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CustomerViewModel model)
        {
            model.Id = id;
            try
            {
                if (ModelState.IsValid)
                {
                    var customer = CustomerMapper.MapCustomerFromModel(model);
                    Service.Update(customer);
                    return RedirectToAction("Index");
                }
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
            catch
            {
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }
        //
        // GET: /Customer/Delete/5
        public ActionResult Delete(int id)
        {
            var customer = Service.GetById(id);
            var model = CustomerMapper.MapCustomerToModel(customer);
            return View("Delete", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }

        //
        // POST: /Customer/Delete/5
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
                var customer = Service.GetById(id);
                var model = CustomerMapper.MapCustomerToModel(customer);
                ModelState.AddModelError("", e.Message);
                return View("Delete", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }
    }
}
