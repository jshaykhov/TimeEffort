using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Models;
using TimeEffort.Mappers;
using TimeEffortCore.Entities;
using TimeEffortCore.Services;
using System.Web.Security;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

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
        public ActionResult Login(LoginViewModel loginVM, FormCollection collection, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(loginVM);
            try
            {
                var curUser = new UserInfo
                {
                    Username = loginVM.UserName.Trim().ToLower(),
                    Password = loginVM.Password
                };
                if (_userService.Authenticate(curUser).HasValue)
                {
                    var cookie = CreateTicket(loginVM.UserName, loginVM.RememberMe);
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);

                    if (returnUrl == null || returnUrl == "")
                        return RedirectToAction("Index", "Home");
                    return Redirect(returnUrl);
                }

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

        private HttpCookie CreateTicket(string username, bool remember)
        {
            var authTicket = new FormsAuthenticationTicket(
                1,                                                      //VERSION
                username,
                DateTime.Now,
                DateTime.Now.AddMinutes(20),
                remember,
                _userService.GetUserByUsername(username).Position.Name  //ROLE OF THE USER
            );
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            return authCookie;
        }


        //REGISTER 
        public ActionResult Registration()
        {
            CreateSelectListForDropDown();
            return View();
        }

// POST: User/Register
        [HttpPost]
        public ActionResult Registration(RegistrationViewModel registrationVM)
        {
            if (!ModelState.IsValid)
            {
               
                return View(registrationVM);
            }
            try
            {

                var curUser = new UserInfo
                {
                    FirstName = registrationVM.FirstName,
                    LastName = registrationVM.LastName,
                    Username = registrationVM.UserName.Trim().ToLower(),
                    Password = registrationVM.Password,
                    Phone = registrationVM.Phone,
                    PositionID = registrationVM.PositionId,
                    Email = registrationVM.Email
                };
                _userService.Register(curUser);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                CreateSelectListForDropDown();
                ModelState.AddModelError("", ex.Message);
                return View(registrationVM);
            }
        }

        private void CreateSelectListForDropDown()
        {
            //Create selectlist of positions 
            //to pass it to view's dropdown
            //to allow user selection during registration
            var listOfPositions = _userService.GetAllPositions();
            SelectList positions = new SelectList(PositionMapper.MapPositionsToModels(listOfPositions),
                                                   "Id ", "Position");
            //store list of positions in ViewBag 
            //for further use in view's dropdown list
            ViewBag.Positions = positions;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }


    }
}