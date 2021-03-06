﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Helper;
using TimeEffort.Models;
using TimeEffort.DAL;

namespace TimeEffort.Controllers
{
    [Authorize(Roles = "Admin, Master, Monitor, User,CTO, Test")]
    public class NotificationController : Controller
    {
        //static properties does not requiere instantiation on accessing different Actions
        static AllDBServices _Service;

        //static UserService UserService;
        //singleton to ensure that there is only one instance of the service

        private AllDBServices db
        {
            get
            {
                if (_Service == null)
                    _Service = new AllDBServices();
                return _Service;
            }
        }
        //
        // GET: /Notification/
        public ActionResult Index()
        {
            int id = db.GetUserByUsername(User.Identity.Name).ID;
            var notifications = db.GetNotificationsForUser(id, DateTime.Today, false);

            var list = notifications.Select(c => new NotificationViewModel
            {
                ID = c.ID,
                Date = c.Date,
                FROMID = c.FROMID,
                ISREAD = c.ISREAD,
                MESSAGE = c.MESSAGE,
                ProjectId = c.ProjectId,
                TOID = c.TOID,
                TYPEID = c.TYPEID,
                Project = c.Project,
            }).ToList();

            return View("Index", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", list);
        }

        [ChildActionOnly]
        public ActionResult GetUnreadNotification()
        {
            var notifications = db.GetNotificationsForUser(db.GetUserByName(User.Identity.Name).ID, DateTime.Today);
            notifications.OrderByDescending(x => x.Date);

            var list = notifications.Select(c => new NotificationViewModel
            {
                ID = c.ID,
                Date = c.Date,
                FROMID = c.FROMID,
                ISREAD = c.ISREAD,
                MESSAGE = c.MESSAGE,
                ProjectId = c.ProjectId,
                TOID = c.TOID,
                Project = c.Project,
                TYPEID = c.TYPEID
            }).ToList();

            return PartialView("_NotificationPartial", list);
            //return Json(notifications, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult MarkAsRead(List<String> data)
        {
            int temp = 0; bool successfully = false;
            foreach (string i in data)
            {
                if (int.TryParse(i, out temp))
                    successfully = db.MarkNotificationAsReadSuccessfully(temp);
            }


            if (successfully == true)
                return Json(new { message = "Successfully changed" }, JsonRequestBehavior.DenyGet);
            else
                return Json(new { message = "One or more selected values changing failed" }, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        public ActionResult DeleteSelected(List<String> data)
        {
            int temp = 0; bool successfully = false;
            foreach (string i in data)
            {
                if (int.TryParse(i, out temp))
                    successfully = db.DeleteSelectedNotificationSuccessfully(temp);
            }


            if (successfully == true)
                return Json(new { message = "Successfully deleted", success = true}, JsonRequestBehavior.DenyGet);
            else
                return Json(new { message = "One or more records deleting failed", success = false }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult MarkAllAsRead()
        {
            bool successfully = db.MarkAllAsReadSuccessfully(User.Identity.Name);
            
            if (successfully == true)
                return Json(new { message = "Successfully marked", success = true }, JsonRequestBehavior.DenyGet);
            else
                return Json(new { message = "One or more records marking failed", success = false }, JsonRequestBehavior.DenyGet);
        }
    }
}