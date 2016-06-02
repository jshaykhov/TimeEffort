using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeEffort.Controllers
{
    public class CalendarController : Controller
    {
        //
        // GET: /Calendar/
        [HttpGet]
        public ActionResult Index(string dateClicked )
        {
            return View();
        }
	}
}