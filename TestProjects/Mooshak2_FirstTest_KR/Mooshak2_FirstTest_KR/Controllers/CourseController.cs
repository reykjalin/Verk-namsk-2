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

        public ActionResult Index() { return RedirectToAction("List"); }

        [HttpGet]
        public ActionResult create()
        {
            return View();
        }
        

        /// <summary>
        /// Edit course with ID 'id'. Shows the edit view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult edit(int? id)
        {
            if(id.HasValue)
            {
                var model = service.getCourseById(id);
                if(model != null)
                    return View(model);
            }
            return RedirectToAction("Error");
        }

        [HttpPost]
        public ActionResult edit(CourseViewModel course)
        {
            // Athuga hvort input sé valid
            if(ModelState.IsValid)
            {
                // Uppfæra upplýsingar í course
                if(service.updateCourse(course))
                {
                    return RedirectToAction("List");
                }
            }
            // Ef input ekki valid, sýna view aftur
            return View(course);
        }

        /// <summary>
        /// Removes course with ID 'id' from database.
        /// </summary>
        /// <returns></returns>
        public ActionResult remove(int? id)
        {
            if(id.HasValue)
            {
                // TODO: Implement
                return View();
            }
            return RedirectToAction("Error");
        }

        public ActionResult details(int? id)
        {
            if(id.HasValue)
            {
                var model = service.getCourseById(id);
                if(model != null)
                    return View(model);
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Shows a page with a list of courses available in the database.
        /// </summary>
        /// <returns></returns>
        public ActionResult list()
        {
            var model = service.getAllCourses();
            return View(model);
        }

        public ActionResult error() { return View(); }
    }
}