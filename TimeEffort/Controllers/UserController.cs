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
                    FormsAuthentication.SetAuthCookie(curUser.Username, false);
                    if (returnUrl == null || returnUrl == "")
                        return RedirectToAction("Index","Home");

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
                CreateSelectListForDropDown();
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

    }
  }
