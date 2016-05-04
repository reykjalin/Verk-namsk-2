using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mooshak2_FirstTest_KR.Models.ViewModels;
using Mooshak2_FirstTest_KR.Services;

namespace Mooshak2_FirstTest_KR.Controllers
{
    public class CourseController : Controller
    {
        private CourseService service;

        public CourseController() { service = new CourseService(); }

        [HttpGet]
        public ActionResult create()
        {
            // Data should be of type CourseViewModel
            var data = service.getCourseById(1);
            return View(data);
        }

        public ActionResult edit()
        {
            var model = service.getCourseById(1);
            return View(model);
        }

        public ActionResult remove() { return View(); }

        public ActionResult details(int id) { return View(); }

        public ActionResult courseList()
        {
            var model = service.getAllCourses();
            return View();
        }
    }
}