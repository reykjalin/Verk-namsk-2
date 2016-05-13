using MooshakV2.Services;
using MooshakV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace MooshakV2.Controllers
{
    public class CourseController : Controller
    {
        private CourseService service;
		private AssignmentService assService;
        private UserService userService;

        public CourseController()
		{
			service = new CourseService();
			assService = new AssignmentService();
            userService = new UserService();
		}
		
		/// <summary>
        /// Shows the listview
		/// </summary>
		/// <returns></returns>
		public ActionResult Index() { return RedirectToAction("List"); }

        /// <summary>
        /// Shows Create view for Course
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult create()
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var model = new CourseDetailViewModel();
            model.studentList = userService.getAllStudents(userManager);
            model.taList = userService.getAllTAs(userManager);
            model.teacherList = userService.getAllTeachers(userManager);
            return View("AdminTeacherViews/create", model);
        }

        /// <summary>
        /// adds Course 'newCourse' to the database.
        /// </summary>
        /// <param name="newCourse"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult create(CourseDetailViewModel newCourse)
        {
            
            // Ignores errors that are from invalid id, DB makes sure to generate an id
            ModelState["course.id"].Errors.Clear();
            if (ModelState.IsValid)
            {
                // adds newCourse to the database if the input is valid.
                if (service.addCourse(newCourse.course))
                    return RedirectToAction("List");
            }
            // If input is invalid, show the same view with error the message 
            return View("AdminTeacherViews/create", newCourse);
        }


        /// <summary>
        /// Change Course with ID 'id'. shows the Edith view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult edit(int? id, string query)
        {
            // Checks whether id is NULL
            if (id.HasValue)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var model = new CourseDetailViewModel();
                model.course = service.getCourseById(id);
                model.assignmentList = assService.getAllAssignmentsInCourse(id.Value);
                model.taList = userService.getAllTAs(userManager);
                model.teacherList = userService.getAllTeachers(userManager);

                if (query == null)
                    model.studentList = userService.getAllStudents(userManager);
                else
                    model.studentList = userService.searchForUser(query, userManager);

                // If Course exist , send model to view, else error
                if (model != null)
                    return View("AdminTeacherViews/edit", model);
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// changes Course in DB with the same ID and the 'Course' with updated data from 'Course'
        /// </summary>
        /// <param name="toEdit"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult edit(CourseDetailViewModel toEdit)
        {
            // Checks if input is valid
            if (ModelState.IsValid)
            {
                // updates information in course
                if (service.updateCourse(toEdit.course))
                    return RedirectToAction("List");
            }
            // If input is not valid, show view again
            return View("AdminTeacherViews/edit", toEdit);
        }

        /// <summary>
        /// site thats askes for the conformation about whether is should delete Course with ID 'id'.
        /// goes to error site if course is not available, or id is null.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult remove(int? id)
        {
            // checks whether id is null
            if (id.HasValue)
            {
                // finds Course to delete
                var toRemove = service.getCourseById(id);

                // if Course exist so conformation view, other wise error.
                if (toRemove != null)
                    return View("AdminTeacherViews/remove", toRemove);
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// delets course with ID 'toRemove.id' from database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult remove(CourseViewModel toRemove)
        {
			var assList = assService.getAllAssignmentsInCourse(toRemove.id);
			foreach(var item in assList)
			{
				assService.removeAssignment(item.id);
			}
            // toRemove delete from DB, if something goes wrong error message will be shown
            if (service.removeCourse(toRemove.id))
                return RedirectToAction("List");

            return RedirectToAction("Error");
        }

        /// <summary>
        /// Sýnir nánari upplýsingar um Course með ID 'id'
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher, Student")]
        public ActionResult details(int? id)
        {
            // Athuga hvort id sé null
            if (id.HasValue)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                CourseDetailViewModel model = new CourseDetailViewModel();
				// Course með ID 'id' fundinn
				model.course = service.getCourseById(id); ;
				// Assignment í Course með ID 'id' fundinn
				model.assignmentList = assService.getAllAssignmentsInCourse(id.Value);
                // Get students
                model.studentList = userService.getAllStudents(userManager);
                // Get TAs
                model.taList = userService.getAllTAs(userManager);
                // Get teachers
                model.teacherList = userService.getAllTeachers(userManager);

                // Ef Course er til, birta Details view, annars sýna Error view.
                if(model != null)
                {
                    if(User.IsInRole("Student"))
                        return View("StudentViews/details", model);

                    return View("AdminTeacherViews/details", model);
                }
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Sýnir lista af öllum Courses í gagnagrunni.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Teacher, Student")]
        public ActionResult list()
        {
            // Fá lista af öllum Courses og birta hann.
            var model = service.getAllCourses();
            if(User.IsInRole("Student"))
                return View("StudentViews/list", model);
          
            return View("AdminTeacherViews/list", model);
        }

        [HttpPost]
        public ActionResult addUser(int courseId, UserViewModel model)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if(courseId > 0 && model != null)
            {
                if(service.addUserToCourse(courseId, model, userManager))
                    return RedirectToAction("Edit", courseId);
            }
            return View("Error");
        }


        /// <summary>
        /// Sýnir Error view.
        /// </summary>
        /// <returns></returns>
        public ActionResult error() { return View(); }
    }
}