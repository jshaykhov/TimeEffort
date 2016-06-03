﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeEffort.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles="Admin, Master, Monitor, User")]
        public ActionResult Index()
        {
            return View("Index" , masterName:"~/Views/Shared/_Layout.cshtml");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Time Effort web application for the employees of LG CNS Uzbekistan";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}