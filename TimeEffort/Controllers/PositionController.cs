﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeEffort.Helper;
using TimeEffort.Mappers;
using TimeEffort.Models;
using TimeEffortCore.Services;

namespace TimeEffort.Controllers
{
     [Authorize(Roles = "Admin")]
    public class PositionController : Controller
    {
        //static properties does not requiere instantiation on accessing different Actions
        static PositionDBService _Position;
        //singleton to ensure that there is only one instance of the service

        private PositionDBService Position
        {
            get
            {
                if (_Position == null)
                    _Position = new PositionDBService();
                return _Position;
            }
        }
        // GET: /Position/
        
        public ActionResult Index()
        {
            var allPositions = Position.GetAll();
            var list = PositionMapper.MapPositionsToModels(allPositions);
            return View("Index", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml",list);
            //return View(list);
        }

        // GET: Position/Create
        public ActionResult Create()
        {
            var model = new PositionViewModel();
            return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml");
        }

        // POST: Position/Create
        [HttpPost]
        public ActionResult Create(PositionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
             
                    var position = PositionMapper.MapPositionFromModel(model);

                    Position.Insert(position);
                    return RedirectToAction("Index");
                }

                return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View("Create", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml");
            }
        }
        // GET: Position/Edit/5
        public ActionResult Edit(int id)
        {
            var model = PositionMapper.MapPositionToModel(Position.GetById(id));
            return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
        }

        // POST: Position/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PositionViewModel model)
        {
            model.Id = id;
            try
            {
                if (ModelState.IsValid)
                {
                    var position = PositionMapper.MapPositionFromModel(model);
                    Position.Update(position);
                    return RedirectToAction("Index");
                }
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
            catch
            {
                return View("Edit", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }

        // GET: Position/Delete/5
        public ActionResult Delete(int id)
        {
            var position = Position.GetById(id);
            var model = PositionMapper.MapPositionToModel(position);
            return View("Delete", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml",model);
         
        }

        // POST: Position/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Position.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var position = Position.GetById(id);
                var model = PositionMapper.MapPositionToModel(position);
                ModelState.AddModelError("", e.Message);
                return View("Delete", "~/Views/Shared/_Layout" + HelperUser.GetRoleName(User) + ".cshtml", model);
            }
        }
    }
}
