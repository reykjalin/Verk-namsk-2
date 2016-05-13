using System;
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
    public class UserController : Controller
    {
        private UserService service;

        /// <summary>
        /// Constructor, initializes the user service object.
        /// </summary>
        public UserController() { service = new UserService(); }

        /// <summary>
        /// Redirects to the List view, making the list view the default course view
        /// </summary>
        /// <returns>List view</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult index() { return RedirectToAction("List"); }

        /// <summary>
        /// Show the create view, GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult create()
        {
            prepareDropDown();
            return RedirectToAction("Register", "Account");
        }

        /// <summary>
        /// Show the edit view, GET
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult edit(string userName)
        {
            if(!userName.IsEmpty())
            {
                // userManager used to get user role information
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var model = new UserDetailViewModel();
                // Get user info
                model = service.getUserByUserName(userName, userManager);
                if(model != null)
                {
                    prepareDropDown();
                    return View(model);
                }
            }
            return View("Error");
        }

        /// <summary>
        /// Edit POST method. Shows the edit view with errors if editing fails
        /// </summary>
        /// <param name="changedUser"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult edit(UserDetailViewModel changedUser)
        {
            prepareDropDown();
            if(ModelState.IsValid)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                if(service.changeUser(changedUser, userManager))
                    return RedirectToAction("List");
            }
            return View(changedUser);
        }

        /// <summary>
        /// Returns the list view, GET
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult list(string search)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var model = new List<UserDetailViewModel>();
            // Decide what user list to show depending on what the search query is
            if(search == null)
                model = service.getAllUsers(userManager);
            else
                model = service.searchForUser(search, userManager);
            return View(model);
        }

        /// <summary>
        /// Returns the details view, GET. Now obselete after adding modal windows
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult details(string userName)
        {
            if(!userName.IsEmpty())
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                // Get user info
                var model = service.getUserByUserName(userName, userManager);
                if(model != null)
                {
                    if(User.IsInRole("Student"))
                        return View("StudentViews/details", model);
                    return View(model);
                }
            }
            return View("Error");
        }

        /// <summary>
        /// Shows the delete view, GET. Shows a confirmation window before deleting a course.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult delete(string userName)
        {
            if(!userName.IsEmpty())
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                // Get user info
                var model = service.getUserByUserName(userName, userManager);
                if(model != null)
                    return View(model);
            }
            return View("Error");
        }

        /// <summary>
        /// Delete course, POST. Shows error view if it fails
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult delete(UserDetailViewModel toRemove)
        {
            if(toRemove != null)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                if(service.deleteUser(toRemove, userManager))
                    return RedirectToAction("List");
            }
            return View("Error");
        }

        /// <summary>
        /// Prepares drop down ViewData for views.
        /// </summary>
        private void prepareDropDown()
        {
            // Get drop down data with role info for edit and create views
            var roleList = service.getRoles();
            List<SelectListItem> roleDropDown = new List<SelectListItem>();

            foreach (var item in roleList)
                roleDropDown.Add(new SelectListItem { Text = item.Name, Value = item.Name });

            ViewData["roleList"] = roleDropDown;
        }
    }
}