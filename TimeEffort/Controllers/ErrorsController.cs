﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeEffort.Controllers
{
    public class ErrorsController : Controller
    {
        //
        // GET: /Errors/
        public ActionResult NotFound()
        {
            ActionResult result;

            object model = Request.Url.PathAndQuery;

            if (!Request.IsAjaxRequest())
                result = View(model);
            else
                result = PartialView("Error", model);

            return result;
        }
	}
}