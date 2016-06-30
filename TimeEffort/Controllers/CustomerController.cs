using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Helper;
using TimeEffort.Mappers;
using TimeEffort.Models;
using TimeEffortCore.Services;
using PagedList;
using PagedList.Mvc;

namespace TimeEffort.Controllers
{
    [Authorize(Roles = "Admin, Master, Monitor,CTO, Test")]
    public class CustomerController : Controller
    {
        static AllDBServices _Service;
        private AllDBServices Service
        {
            get
            {
                if (_Service == null)
                    _Service = new AllDBServices();
                return _Service;
            }
        }
        //
        // GET: /Customer/
        public ActionResult Index(int? page, string currentFilter, string searchByCName)
        {
            var model = new CustomerModel();
            var allCustomers = Service.GetAllCustomers().Where(c=>c.ID!=0).ToList();
            var list = CustomerMapper.MapCustomersToModels(allCustomers);
            if (searchByCName != null)
                page = 1;
            else
                searchByCName = currentFilter;

            ViewBag.CurrentFilter = searchByCName;
            if (!String.IsNullOrEmpty(searchByCName))
            {
                list = list.Where(p => p.Name.ToLower().Contains(searchByCName.ToLower())).ToList();
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            model.Pagination = list.ToPagedList(pageNumber, pageSize);
            model.CustomerList = list;
            return View("Index", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
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
            var model = CustomerMapper.MapCustomerToModel(Service.GetCustomerById(id));
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
            catch (Exception e)
            {
                ModelState.AddModelError("", "THE TIN YOU ENTERED ALREADY EXISTS IN THE DATABASE."+" " + e.Message);
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }
        //
        // GET: /Customer/Delete/5
        public ActionResult Delete(int id)
        {
            var customer = Service.GetCustomerById(id);
            if (customer.ID == 0)
               return RedirectToAction("Index");
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
                Service.DeleteCustomer(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var customer = Service.GetCustomerById(id);
                var model = CustomerMapper.MapCustomerToModel(customer);
                ModelState.AddModelError("", "This customer is a client at one or more projects. Thus deleting failed. " + e.Message);
                return View("Delete", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }
    }
}
