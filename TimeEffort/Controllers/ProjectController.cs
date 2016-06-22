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
using System.Xml.Schema;

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
            CreateSelectListForDropDownCustomer();
            CreateSelectListForDropDownUsers();
            var model = new ProjectViewModel();
            return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);

        }
        
          [HttpGet]
          public ActionResult GetCodes(ProjectViewModel model)
          {
             ProjectsAndManagersModel myModel = new ProjectsAndManagersModel();
              List<ProjectViewModel> list = new List<ProjectViewModel>();
              var listOfUsers = UserMapper.MapUsersToModels( Service.GetAllUsers());
            
              var codes = Service.GetNextCode();
              foreach (var code in codes)
              {
                  list.Add(new ProjectViewModel {Id=0,
                                                Code=code, 
                                                CustomerId=model.CustomerId, 
                                                CMoneyUsd=model.CMoneyUsd/4,
                                                CMoneyUzs=model.CMoneyUzs/4,
                                                StartDate=model.StartDate,
                                                FinishDate=model.FinishDate,
                                                PManagerId=model.PManagerId,
                                                ProjectName=model.ProjectName
                  });
              }
              myModel.Projects = list;
              myModel.Users = listOfUsers;
              return Json(myModel, JsonRequestBehavior.AllowGet);
          }
         
        //
        // POST: /Project/Create
        [HttpPost]
        public ActionResult CreateProjects(List<ProjectCreateViewModel> model)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                foreach (var i in model)
                {
                    if(i.StartDate<=DateTime.Now && i.FinishDate>=DateTime.Now)
                        i.Status = "Active";
                    else if (i.StartDate > DateTime.Now)
                        i.Status = "Preparing";
                    if (i.FinishDate < DateTime.Now)
                        i.Status = "Completed";
                    var project = ProjectMapper.MapProjectFromCreateModel(i);
                    
                    //Service.GetNextCode(model.CType);
                    Service.Insert(project);
                }
                return Json(new { msg = "" });
                //}

                //CreateSelectListForDropDownUsers();
                //CreateSelectListForDropDownStatus();
                //return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return Json(new { msg = e.Message });
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
                ModelState.AddModelError("", "You cannot delete this project, because there are records of employee efforts for this project.");
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
            SelectList items = new SelectList(new List<String>() {"Preparing", "Active", "Completed"});
            //store list of users in ViewBag 
            //for further use in view's dropdown list
            ViewBag.Items = items;
        }

        private void CreateSelectListForDropDownCustomer()
        {
            var listOfCustomers = Service.GetAllCustomers();
            SelectList customers = new SelectList(CustomerMapper.MapCustomersToModels(listOfCustomers),
                                                   "Id ", "Name");
            //store list of users in ViewBag 
            //for further use in view's dropdown list
            ViewBag.Customers = customers;
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
         //EXPORT TO CSV
        public ActionResult ExportToCSV()
        {
                var xDoc = new XDocument();
                xDoc.Add(new XProcessingInstruction("xml-stylesheet", "type='text/xsl' href='/xml/ProjectToCSV.xslt'"));

                xDoc.Declaration = new XDeclaration("1.0", "utf-8", null);
                var projects = Service.GetAll();
                if (projects.Count > 0)
                {
                    var xElement = new XElement("Projects",
                        from project in projects
                        select new XElement("Project",
                            new XElement("Id", project.ID),
                            new XElement("Name", project.Name),
                            new XElement("Code", project.Code),
                            new XElement("ContactUSD", project.ContractUSD),
                            new XElement("ContractUZS", project.ContractUZS),
                            new XElement("Manager", project.UserInfo.FirstName),
                            new XElement("StartDate", project.StartDate),
                             new XElement("EndDate", project.EndDate)
                           
                            ));
                    xDoc.Add(xElement);
                }
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add("", "http://" + System.Web.HttpContext.Current.Request.Url.Authority + "/xml/ProjectSchema.xsd");
                var errors = false;
                var eMessage = "";
                xDoc.Validate(schemas, (o, e) =>
                {
                    eMessage = e.Message;
                    errors = true;
                }, true);
                if (errors)
                    xDoc = new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement("Error", eMessage));

                StringWriter sw = new StringWriter();

                xDoc.Save(sw);
                var context = System.Web.HttpContext.Current;
                context.Response.Clear();
                context.Response.Write(sw.ToString());
                context.Response.ContentType = "text/xml";
                context.Response.End();
                return View();
            }

        
            
          
        }
}


