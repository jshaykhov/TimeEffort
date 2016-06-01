using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Models;
using TimeEffortCore.Entities;
using TimeEffortCore.Services;

namespace TimeEffort.Controllers
{
    public class UserController : Controller
    {
        static UserService _userService = new UserService();

        // GET: User/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: User/Login
        [HttpPost]
        public ActionResult Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);
            try
            {
                var curUser = new UserInfo
                {
                    Username = loginVM.UserName,
                    Password = loginVM.Password
                };
                if (_userService.Authenticate(curUser).HasValue)
                    return RedirectToAction("Index", "Home");
                else
                {
                    ModelState.AddModelError("", "Invalid credentials");
                    return View(loginVM);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(loginVM);
            }

        }
        //REGISTER 
         public ActionResult Registration()
        {
            return View();
        }

        // POST: User/Register
        [HttpPost]
        public ActionResult Registration(RegistrationViewModel registrationVM)
        {
            if (!ModelState.IsValid)
                return View(registrationVM);
            try
            {
                var curUser = new UserInfo
                {
                    FirstName = registrationVM.FirstName,
                    LastName = registrationVM.LastName,
                    Username = registrationVM.UserName,
                    Password = registrationVM.Password,
                    Phone=registrationVM.Phone,
                    PositionID=registrationVM.PositionId,                    
                    Email = registrationVM.Email
                };
                _userService.Register(curUser);
                return RedirectToAction("Login");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(registrationVM);
            }
        }

        }
  }
