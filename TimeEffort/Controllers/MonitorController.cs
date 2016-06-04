using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Models;
using TimeEffortCore.Services;

namespace TimeEffort.Controllers
{
    public class MonitorController : Controller
    {
        //---------------Singleton------------------
        public static WorkloadDbService _db;

        public static WorkloadDbService db
        {
            get
            {
                if (_db == null)
                    _db = new WorkloadDbService();
                return _db;
            }
        }
        //---------------Singleton------------------

        //
        // GET: /Monitor/
        public ActionResult Index()
        {
            return View();
        }

        public MonitorViewModel GetResult()
        {
            var result = new MonitorViewModel();

            

            return result;
        }

        private List<ProjectMontior> GetProjects(DateTime from, DateTime to, string? User)
        {
            var result = new List<ProjectMontior>();

            var allProjects = db.GetAll().Where(x => x.Date >= from && x.Date <= to).ToList();
            
            

            return result;
        }
	}
}