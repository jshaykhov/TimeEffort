using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Helper;

namespace TimeEffort.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles="Admin, Master, Monitor, User")]
        public ActionResult Index()
        {
            //
            return View("Index" + HelperUser.GetRoleName(User), masterName: "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}