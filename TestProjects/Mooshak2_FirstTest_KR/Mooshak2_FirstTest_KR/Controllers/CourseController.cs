using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mooshak2_FirstTest_KR.Services;

namespace Mooshak2_FirstTest_KR.Controllers
{
    public class CourseController : Controller
    {
        private CourseService service;

        public CourseController() { service = new CourseService(); }

        public ActionResult create() { return View(); }

        public ActionResult edit() { return View(); }

        public ActionResult remove() { return View(); }

        public ActionResult details(int id) { return View(); }

        public ActionResult courseList() { return View(); }
    }
}