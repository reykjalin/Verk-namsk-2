using Mooshak_TestByE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooshak_TestByE.Controllers
{
    public class CourseController : Controller
    {
        private CourseService service;
        public CourseController()
        {
            service = new CourseService();
        }

        //About course
        public ActionResult course(int courseId)
        {
            return View();
        }

        //Create course
        public ActionResult create()
        {
            return View();
        }

        //Change course
        public ActionResult edit()
        {
            return View();
        }

        //Remove course
        public ActionResult Remove()
        {
            return view();
        }

        private ActionResult view()
        {
            throw new NotImplementedException();
        }

        //Get details about the given id
        public ActionResult details(int id)
        {
            return View();
        }

        //Get all courses
        public ActionResult allCourses()
        {
            return View();
        }
    }
}