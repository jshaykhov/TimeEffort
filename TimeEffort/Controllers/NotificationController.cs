using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Helper;
using TimeEffort.Models;
using TimeEffortCore.Services;

namespace TimeEffort.Controllers
{
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
            int id = HelperUser.GetUserByName(User.Identity.Name).ID;
            var notifications = db.GetNotificationsForUser(id, DateTime.Today);
            return View();
        }

        [ChildActionOnly]
        public ActionResult GetUnreadNotification()
        {
            var notifications = db.GetNotificationsForUser(HelperUser.GetUserByName(User.Identity.Name).ID, DateTime.Today);
            notifications.OrderByDescending(x => x.Date);
            return PartialView("_NotificationPartial", notifications);
            //return Json(notifications, JsonRequestBehavior.DenyGet);
        }
	}
}