using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Helper;

namespace TimeEffort.Controllers
{
    [Authorize(Roles = "Admin, Master, Monitor, User,CTO, Test")]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            //
            return View("Index" + HelperUser.GetRoleName(User), masterName: "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml");
        }

       
        public ActionResult About()
        {
            ViewBag.Message = "Time Effort web application for the employees of LG CNS Uzbekistan";

            return View("About", masterName: "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml");
        }
      
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View("Contact", masterName: "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml");
        }
    }
}