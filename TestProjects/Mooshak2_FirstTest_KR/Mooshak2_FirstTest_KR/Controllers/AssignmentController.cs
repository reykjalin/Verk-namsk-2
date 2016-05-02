using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mooshak2_FirstTest_KR.Services;
using System.Web.Mvc;

namespace Mooshak2_FirstTest_KR.Controllers
{
    public class AssignmentController : Controller
    {
        private AssignmentService service;

        public AssignmentController() { service = new AssignmentService(); }

        public ActionResult create() { return View(); }

        public ActionResult edit() { return View(); }

        public ActionResult remove() { return View(); }

        public ActionResult details(int id) { return View(); }

        public ActionResult assignmentList() { return View(); }
    }
}