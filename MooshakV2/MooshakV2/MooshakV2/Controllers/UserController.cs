﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using MooshakV2.Services;
using MooshakV2.ViewModels;
using Microsoft.AspNet.Identity.Owin;

namespace MooshakV2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private UserService service;
        public UserController() { service = new UserService(); }

        public ActionResult index() { return RedirectToAction("List"); }

        public ActionResult create()
        {
            prepareDropDown();
            return RedirectToAction("Register", "Account");
        }

        [HttpGet]
        public ActionResult edit(string userName)
        {
            if(!userName.IsEmpty())
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var model = service.getUserByUserName(userName, userManager);
                if(model != null)
                {
                    prepareDropDown();
                    return View(model);
                }
            }
            return View("Error");
        }

        [HttpPost]
        public ActionResult edit(UserViewModel changedUser)
        {
            if(ModelState.IsValid)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                if(service.changeUser(changedUser, userManager))
                    return RedirectToAction("List");
            }
            return View(changedUser);
        }

        public ActionResult list()
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var model = service.getAllUsers(userManager);
            return View(model);
        }

        private void prepareDropDown()
        {
            // Búa til drop-down lista með role-um fyrir edit view
            var roleList = service.getRoles();
            List<SelectListItem> roleDropDown = new List<SelectListItem>();

            foreach (var item in roleList)
                roleDropDown.Add(new SelectListItem { Text = item.Name, Value = item.Id });

            ViewData["roleList"] = roleDropDown;
        }
    }
}