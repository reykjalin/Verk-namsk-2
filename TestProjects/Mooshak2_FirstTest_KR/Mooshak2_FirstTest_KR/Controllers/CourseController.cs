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

        /// <summary>
        /// Returns the view of the course list.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() { return RedirectToAction("List"); }

        /// <summary>
        /// Shows view for creating a course
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult create()
        {
            return View();
        }

        /// <summary>
        /// Adds 'newCourse' to the database
        /// </summary>
        /// <param name="newCourse"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult create(CourseViewModel newCourse)
        {
            // TODO: Er meira elegant leið til að hundsa id validation?
            // Hunsa villur sem koma til vegna invalid id, DB sér um að generate-a id
            ModelState["id"].Errors.Clear();
            if(ModelState.IsValid)
            {
                // Bæta newCourse við DB ef input er valid
                if(service.addCourse(newCourse))
                    return RedirectToAction("List");
            }
            // Ef input er invalid, sýna sama view með villuskilaboðum
            return View(newCourse);
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

        /// <summary>
        /// Edits course in DB with same ID as 'course' using data from 'course'
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult edit(CourseViewModel course)
        {
            // Athuga hvort input sé valid
            if(ModelState.IsValid)
            {
                // Uppfæra upplýsingar í course
                if(service.updateCourse(course))
                    return RedirectToAction("List");
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

        /// <summary>
        /// Show detail page of course with ID 'id'
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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