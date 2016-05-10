using MooshakV2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MooshakV2.Controllers
{
    public class SubmissionController : Controller
    {
        private SubmissionService service;

        public SubmissionController()
        {
            service = new SubmissionService();
        }

        [HttpGet]
        public ActionResult create()
        {
            return View("create");
        }
    }
}