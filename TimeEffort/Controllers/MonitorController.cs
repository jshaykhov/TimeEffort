using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Helper;
using TimeEffort.Models;
using TimeEffortCore.Services;
using System.IO;
using System.Data;
using ClosedXML.Excel;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Xml.Linq;
using System.Xml.Schema;

namespace TimeEffort.Controllers
{
    [Authorize(Roles = "Admin, Master, Monitor, User,CTO, Test")]
    public class MonitorController : Controller
    {
        //---------------Singleton------------------
        public static AllDBServices _db;

        public static AllDBServices db
        {
            get
            {
                if (_db == null)
                    _db = new AllDBServices();
                return _db;
            }
        }
        //---------------Singleton------------------

        //
        // GET: /Monitor/
        public ActionResult Index()
        {
            var model = new MonitorViewModel();
            if (!User.IsInRole("User"))
            {
                model.allEmployees = db.GetAllUsers();
                model.allProjects = db.GetAllProjects();
                model.workloads = db.GetAllWorkloadTypes();
            }
            else
            {
                var employees = new List<TimeEffortCore.Entities.UserInfo>(); employees.Add(db.GetUserByName(User.Identity.Name));
                model.allEmployees = employees;
                model.allProjects = db.GetAllInvolvedUserPMProjects(User.Identity.Name);
                model.workloads = db.GetAllWorkloadTypes();
            }
            return View("Index", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }

        [HttpGet]
        public ActionResult MyAjaxRequest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MyAjaxRequest(QueryJson myQuery)
        {
            DateTime from = DateTime.Now.AddDays(-7), to = DateTime.Now;
            if (DateTime.TryParse(myQuery.FromDate, out from))
                from = from;
            if (DateTime.TryParse(myQuery.ToDate, out to))
                to = to;


            MonitorViewModel model = new MonitorViewModel
            {
                query = new QueryMonitor
                {
                    Employee = myQuery.SelectedUser,
                    Project = myQuery.SelectedProject,
                    WorkloadType = myQuery.SelectedType,
                    FromDate = from,
                    ToDate = to
                }
            };
            model = GetResult(model);
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        public MonitorViewModel GetResult(MonitorViewModel model)
        {

            model.projects = GetProjects(DateTime.Now, model.query.FromDate, model.query.ToDate, model.query.Employee, model.query.Project, model.query.WorkloadType);

            return model;
        }

        private List<ProjectMontior> GetProjects(DateTime now, DateTime? from = null, DateTime? to = null, string user = "all", string project = "all", string type = "all")
        {
            if (from == null)
                from = now.AddDays(-7);
            if (to == null)
                to = now;


            var allWorkloads = db.GetAllWorkloads().FindAll(x => x.Date >= from && x.Date <= to).ToList();
            if (!user.Equals("All"))
            {
                allWorkloads = allWorkloads.FindAll(x => x.UserInfo.Username == user).ToList();
            }

            if (!project.Equals("All"))
            {
                var tempProject = db.GetProjectByCode(project);
                allWorkloads = allWorkloads.FindAll(x => x.ProjectID == tempProject.ID).ToList();
            }
            if (!type.Equals("All"))
            {
                var tempWorkload = db.GetAllWorkloadTypes().FirstOrDefault(w => w.Name == type);
                allWorkloads = allWorkloads.FindAll(x => x.WorkloadTypeID == tempWorkload.ID).ToList();
            }

            return allWorkloads.Select(c => new ProjectMontior
            {
                Date = c.Date.ToString(),
                Duration = c.Duration.ToString(),
                Employee = c.UserInfo.FirstName + " " + c.UserInfo.LastName,
                Project = c.Project == null ? "Overhead" : c.Project.Code,
                Type = c.WorkloadType.Name,
                Id = 0
            }).ToList();
        }
        //EXPORT TO CSV
        public ActionResult ExportCSV()
        {
            var xDoc = new XDocument();
            xDoc.Add(new XProcessingInstruction("xml-stylesheet", "type='text/xsl' href='/xml/ProjectToCSV.xslt'"));

            xDoc.Declaration = new XDeclaration("1.0", "utf-8", null);
            var workloads = db.GetAllWorkloads();
            if (workloads.Count > 0)
            {
                var xElement = new XElement("Workloads",
                    from workload in workloads
                    select new XElement("Workload",
                        new XElement("Id", workload.ID),
                        new XElement("Date", workload.Date),
                        new XElement("UserInfo", workload.UserInfo.LastName + workload.UserInfo.FirstName),
                        new XElement("Project", workload.Project == null ? "" : workload.Project.Name),
                        new XElement("Duration", workload.Duration),
                        //new XElement("ApprovedCTO", workload.ApprovedCTO),
                        //new XElement("ApprovedMaster", workload.ApprovedMaster),
                        //new XElement("ApprovedPM", workload.ApprovedPM),
                        new XElement("Note", workload.Note),
                        new XElement("WorkloadType", workload.WorkloadType.Name)

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
        // GET: ExportData
        public ActionResult ExportToExcel()
        {
            // Step 1 - get the data from database
            var data = db.GetAllWorkloads();

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



    }
}