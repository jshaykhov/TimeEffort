using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Mappers;
using TimeEffort.Models;
using TimeEffortCore.Services;
using PagedList;
using TimeEffort.Helper;
using TimeEffortCore.Entities;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Xml.Linq;
using TimeEffort.Utilities;
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
        [Authorize(Roles = "Admin,Master,CTO,Monitor,User")]
        public ActionResult Index(int? page, string currentFilter, string searchByPName)
        {
            var allProjects = new AllUserModel();

            var userProjects = Service.GetAll();

            if (User.IsInRole("User"))
            {                                                                     //If user's role is USER
                userProjects = HelperUser.GetProjectsByWorkingUser(User);        //show him only projects he is involved in
                
            }
            allProjects.PMProjects = ManagedProjects();
            var list = ProjectMapper.MapProjectsToModels(userProjects);
            
            
            if (searchByPName != null)
                page = 1;
            else
                searchByPName = currentFilter;

            ViewBag.CurrentFilter = searchByPName;
            if (!String.IsNullOrEmpty(searchByPName))
            {
                list = list.Where(p => p.ProjectName.ToLower().Contains(searchByPName.ToLower())).ToList();
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            allProjects.UserProjects = list.ToPagedList(pageNumber, pageSize);

            return View(model: allProjects, masterName: "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", viewName: "Index");
        }


        public List<ProjectViewModel> ManagedProjects()
        {
            var allProjects = HelperUser.GetProjectsByManager(User);
            var list = ProjectMapper.MapProjectsToModels(allProjects);
            
            return list;

        }

        //
        // GET: /Project/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
          [Authorize(Roles = "Admin,Master,Monitor,CTO")]
        //
        // GET: /Project/Create
        public ActionResult Create()
        {
            CreateSelectListForDropDownStatus();
            CreateSelectListForDropDownUsers();
            var model = new ProjectViewModel();
            return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);

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
                CreateSelectListForDropDownUsers();
                CreateSelectListForDropDownStatus();
                return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }

        }
          [Authorize(Roles = "Admin,Master,Monitor,CTO")]
        //
        // GET: /Project/Edit/5
        public ActionResult Edit(int id)
        {
            CreateSelectListForDropDownUsers();
            CreateSelectListForDropDownStatus();
            var model = ProjectMapper.MapProjectToModel(Service.GetById(id));
            return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
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
                CreateSelectListForDropDownUsers();
                CreateSelectListForDropDownStatus();
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
            catch
            {
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }

        }
          [Authorize(Roles = "Admin,Master,Monitor,CTO")]
        //
        // GET: /Project/Delete/5
        public ActionResult Delete(int id)
        {
            var project = Service.GetById(id);
            var model = ProjectMapper.MapProjectToModel(project);
            return View("Delete", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
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
                return View("Delete", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }

        }
        private void CreateSelectListForDropDownUsers()
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
        private void CreateSelectListForDropDownStatus()
        {
            SelectList items = new SelectList(new List<String>() {"Active", "Inactive"});
            //store list of users in ViewBag 
            //for further use in view's dropdown list
            ViewBag.Items = items;
        }

         // GET: ExportData
        public ActionResult ExportToExcel()
        {
            // Step 1 - get the data from database
            var data = Service.GetAll();

            // instantiate the GridView control from System.Web.UI.WebControls namespace
            // set the data source
            GridView gridview = new GridView();
            gridview.DataSource = data;
            gridview.DataBind();

            // Clear all the content from the current response
            Response.ClearContent();
            Response.Buffer = true;
            // set the header
            Response.AddHeader("content-disposition", "attachment;filename=itfunda.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            // create HtmlTextWriter object with StringWriter
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    // render the GridView to the HtmlTextWriter
                    gridview.RenderControl(htw);
                    // Output the GridView content saved into StringWriter
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
             }
            return View();

    }
        public void GetCSV()
        {
            var userProjects = Service.GetAll();
            //MemoryStream stream = CSVUtility.GetCSV(userProjects);
            GridView gridview = new GridView();
            gridview.DataSource = userProjects;
            gridview.DataBind();

            var filename = "ExampleCSV.csv";
            var contenttype = "text/csv";
            Response.Clear();
            Response.ContentType = contenttype;
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.BinaryWrite(gridview.ToArray());
            Response.End();
        }
}
}

