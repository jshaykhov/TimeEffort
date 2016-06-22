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


using Microsoft.Owin.Security;
using PagedList;
using TimeEffort.Helper;
using TimeEffort.App_Start;

namespace TimeEffort.Controllers
{
    public class UserController : Controller
    {
        static AllDBServices _userService = new AllDBServices();    

        // GET: User/Login
        public ActionResult Login()
        {
            //Encrypt.EncryptWebConfig(Request);
            return View();
        }

        // POST: User/Login
        [HttpPost]
        public ActionResult Login(LoginViewModel loginVM, string returnUrl)
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

        [HttpGet]
        public ActionResult LoginAjax()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginAjax(LoginJson query)
        {
            var curUser = new UserInfo
            {
                Username = query.Username.Trim().ToLower(),
                Password = query.Password
            };

            if (_userService.Authenticate(curUser).HasValue)
            {
                var cookie = CreateTicket(query.Username, query.RememberMe.Equals("true"));
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);

                if (query.ReturnUrl == null || query.ReturnUrl == "") { 
                    var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index","Home");
                    return Json(new { Url = redirectUrl });
                }

                query.ReturnUrl = query.ReturnUrl.Replace("%2f", "/");
                return Json(new { Url = query.ReturnUrl });
            }

            else
            {
                return Json(query, JsonRequestBehavior.DenyGet);
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

         [Authorize(Roles = "Admin, Master,CTO")]
        //REGISTER 
        public ActionResult Registration()
        {
            CreateSelectListForDropDown();
            return View("Registration", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml");
        }

// POST: User/Register
        [HttpPost]
        public ActionResult Registration(RegistrationViewModel registrationVM)
        {
            if (!ModelState.IsValid)
            {

                return View("Registration", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", registrationVM);
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
                    Address=registrationVM.Address,
                    Major=registrationVM.Major,
                    PositionID = registrationVM.PositionId,
                    Email = registrationVM.Email
                };
                _userService.Register(curUser);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                CreateSelectListForDropDown();
                ModelState.AddModelError("", ex.Message);
                return View("Registration", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", registrationVM);
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
        //CHANGE PASSWORD
        public ActionResult ChangePassword()
        {
            var model = new ChangePasswordViewModel();
            return View("ChangePassword", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            var newPassword = model.NewPassword;
            var repeatPassword = model.RepeatPassword; 
                        if (!ModelState.IsValid)
                            return View("ChangePassword", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            try
            {
                var curUser = new UserInfo
                {
                    Username=User.Identity.Name,
                    Password = model.CurPassword,
                  };
                        
                if (_userService.Authenticate(curUser).HasValue && newPassword==repeatPassword )
                {
                   curUser.Password = model.NewPassword;
                   bool successfullyChanged = _userService.ChangePassword(curUser);
                   
                    if(successfullyChanged)
                        return RedirectToAction("Login");
                    else
                    {
                        ModelState.AddModelError("", "Could not change, please contact administrator");
                        return View("ChangePassword", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid credentials");
                    return View("ChangePassword", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
                };

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            };

        }
        //List of Users
         [Authorize(Roles = "Admin, Master, CTO")]
        public ActionResult Index(int? page)
        {
            var sendingModel = new SendingModel();
            var allUserLists = new UserViewModel();
            var allUsers = _userService.GetAllUsers();
            var list = UserMapper.MapUsersToModels(allUsers);
            //Add paging
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            sendingModel.Pagination = list.ToPagedList(pageNumber, pageSize);
            sendingModel.UserList = list;
            return View("Index", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", sendingModel);
        }

        //Delete user
         [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var user = _userService.GetUserById(id);
            var model = UserMapper.MapUserToModel(user);
            return View("Delete", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }

        // POST: Position/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                _userService.DeleteUser(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var user = _userService.GetUserById(id);
                var model = UserMapper.MapUserToModel(user);
                ModelState.AddModelError("", e.Message);
                return View("Delete", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }
         [Authorize(Roles = "Admin, Master,CTO")]
        //EDIT
        // GET: Position/Edit/5
        public ActionResult Edit(int id)
        {
            CreateSelectListForDropDown();
            var model = UserMapper.MapUserToModel(_userService.GetUserById(id));
            return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml",model);
        }

        // POST: Position/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, UserViewModel model)
        {
            model.Id = id;
            UserViewModel user1 = UserMapper.MapUserToModel(_userService.GetUserById(model.Id));
            model.Password = user1.Password;
            try
            {
                if (ModelState.IsValid)
                {
                    var user = UserMapper.MapUserFromModel(model);
                    _userService.Update(user);
                    return RedirectToAction("Index");
                }
                CreateSelectListForDropDown();
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
            catch
            {
                //CreateSelectListForDropDown();
                //ModelState.AddModelError("", ex.Message);
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }
          [Authorize(Roles = "Admin, Master, Monitor, User,CTO, Test")]
        public ActionResult UserProfile()
        {
            int id = _userService.GetUserIdByUsername(this.HttpContext.User.Identity.Name);
            var model = UserMapper.MapUserToModel(_userService.GetUserById(id));
            return View("UserProfile", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }
          [Authorize(Roles = "Admin, Master, Monitor, User,CTO, Test")]
        public ActionResult Manage(int id)
        {
            var model = UserMapper.MapUserToModel(_userService.GetUserById(id));
            return View("Manage", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }
        [HttpPost]
        public ActionResult Manage(int id,ProfileViewModel model)
        {
            ProfileViewModel user = UserMapper.MapProfileToModel(_userService.GetUserById(model.Id));
            model.UserName = user.UserName;
            model.PositionId = user.PositionId;
            model.Password = user.Password;
            try
            {
                if (ModelState.IsValid)
                {
                    var curUser = UserMapper.MapProfileFromModel(model);
                    _userService.Update(curUser);
                    return RedirectToAction("UserProfile");
                }
                CreateSelectListForDropDown();
                return View("Manage", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
            catch
            {
                //CreateSelectListForDropDown();
                //ModelState.AddModelError("", ex.Message);
                return View("Manage", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }

    }
}