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
namespace TimeEffort.Controllers
{
    [Authorize(Roles = "Admin, Master, Monitor, User,CTO, Test")]
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
        public ActionResult Index (int? projectId)
        {
            int managerId=Service.GetUserIdByUsername(User.Identity.Name);
            var projects = ProjectMapper.MapProjectsToModels(Service.GetAllProjects().Where(p => p.ManagerID == managerId && p.Status!="Completed").ToList());
            AppointListModel model = new AppointListModel();
            model.Projects = projects;
            if (projects.Count > 0)
                model.selectedProject = projectId == null ? projects.ElementAt(0).Id : (int)projectId;
            return View("Index", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
           
        }
        [HttpPost]
        public ActionResult GetMembers(int projectId)
        { 
            int userId= Service.GetUserIdByUsername(User.Identity.Name);
            List<AppointViewModel> members= AppointMapper.MapAppointsToModels(Service.GetAll().Where(m=>m.ProjectID==projectId && m.UserID!=userId).ToList());
            return Json(members, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult FillDropDowns(int projectId)
        {
            AppointCreateModel model = new AppointCreateModel();
            var involvedUsers = Service.GetAll().Where(p => p.ProjectID == projectId).Select(u=>u.UserID).ToList();
            involvedUsers.Add(Service.GetUserIdByUsername(User.Identity.Name));
            List<UserViewModel> unassignedEmps = UserMapper.MapUsersToModels(Service.GetAllUsers().Where(u => !involvedUsers.Contains(u.ID)).ToList());
            if (unassignedEmps.Count == 0)
                model.Message = "No employees to assign for the project";
            model.Emps = unassignedEmps;
            model.Roles = RoleMapper.MapRolesToModels(Service.GetAllRoles());
            return Json(model, JsonRequestBehavior.DenyGet);
        }
      
        //public ActionResult IsActive(int projectId)
        //{
        //    string isActive = Service.GetAllProjects().FirstOrDefault(p => p.ID == projectId).Status;
        //    string message = "";
        //    if (isActive=="Active")
        //    {
        //        message = "Imposible to assign project members. The project is not active!";
        //    }
        //    return Json(new { msg=message});
        //}
        
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
        public ActionResult Create(int projectId, int empId, int roleId, DateTime dateFrom, DateTime dateTo)
        {
            AppointViewModel model = new AppointViewModel();
            model.ProjectID = projectId;
            model.UserID = empId;
            model.RoleID = roleId;
            model.DateFrom = dateFrom;
            model.DateTo = dateTo;
            
            try
            {
                if (ModelState.IsValid)
                {
                    var appoint = AppointMapper.MapAppointFromModel(model);
                    _Service.Insert(appoint);
                    return RedirectToAction("Index", new { projectId = projectId });
                }
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
                    int projectId = Service.GetById(id).ProjectID;
                    model.ProjectID = projectId;
                    var appoint = AppointMapper.MapAppointFromModel(model);
                    Service.Update(appoint);
                    return RedirectToAction("Index", new { projectId = projectId });
                }
                CreateSelectListForDropDownRoles();
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
            catch (Exception ex)
            {
                
                CreateSelectListForDropDownRoles();
                ModelState.AddModelError("", ex.Message);
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
                int projectId = Service.GetById(id).ProjectID;
                Service.Delete(id);
                return RedirectToAction("Index", new { projectId = projectId});
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

        [HttpGet]
        public ActionResult GetCurProjects(int userId)
        {
            var list = AppointMapper.MapAppointsToModels(Service.GetAll().Where(p=>p.UserID==userId & p.Project.EndDate.Year>=DateTime.Now.Year).ToList());

            object[] arr = ConvertToArray(list);
            return Json(arr, JsonRequestBehavior.AllowGet);
        }
        private object[] ConvertToArray(List<AppointViewModel> list)
        {
            string[] colors = new string[] { "#66CC99", "#CCFF66", "#99CCFF", "#CCCC33", "#99FF66", "#FFCC66", "#66CC99", "#CC9999", "#99CCFF", "#66CC99", "#CC9999", "#99CCFF" };
            object[] arr = new object[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                arr[i] = new { start = list.ElementAt(i).DateFrom, title = list.ElementAt(i).ProjectFullName, end = list.ElementAt(i).DateTo, allDay = true, color = colors[i] };
            }
            return arr;
        }
    }
}
