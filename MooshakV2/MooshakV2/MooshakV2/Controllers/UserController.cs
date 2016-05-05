using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MooshakV2.Services;

namespace MooshakV2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private UserService service;
        public UserController() { service = new UserService(); }

        public ActionResult index() { return RedirectToAction("List"); }

        public ActionResult create() { return RedirectToAction("Register", "Account"); }

        [HttpGet]
        public ActionResult edit(int? id)
        {
            if(id.HasValue)
            {
                
            }

            return View("Error");
        }

        public ActionResult list()
        {
            var model = service.getAllUsers();
            return View(model);
        }
    }
}