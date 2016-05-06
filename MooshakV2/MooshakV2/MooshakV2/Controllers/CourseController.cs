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
		/// Sýnir List view-inu
		/// </summary>
		/// <returns></returns>
		public ActionResult Index() { return RedirectToAction("List"); }

        /// <summary>
        /// Sýnir Create view-ið fyrir Course
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult create()
        {
            return View("AdminTeacherViews/create", new CourseViewModel());
        }

        /// <summary>
        /// Bætir Course 'newCourse' við gagnagrunn.
        /// </summary>
        /// <param name="newCourse"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult create(CourseViewModel newCourse)
        {
            // TODO: Er meira elegant leið til að hundsa id validation?
            // Hunsa villur sem koma til vegna invalid id, DB sér um að generate-a id
            //ModelState["id"].Errors.Clear();
            if (ModelState.IsValid)
            {
                // Bæta newCourse við DB ef input er valid
                if (service.addCourse(newCourse))
                    return RedirectToAction("List");
            }
            // Ef input er invalid, sýna sama view með villuskilaboðum
            return View("AdminTeacherViews/create", newCourse);
        }


        /// <summary>
        /// Breyta Course með ID 'id'. Sýnir Edit View-ið.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult edit(int? id)
        {
            // Athuga hvort id sé null
            if (id.HasValue)
            {
                // Sækja Course með ID 'id'
                var model = service.getCourseById(id);

                // Ef Course er til, senda model á View, annars error
                if (model != null)
                    return View("AdminTeacherViews/edit", model);
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Breytir Course í gagnagrunni með sama ID og 'course' með uppfærðum gögnum úr 'course'
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult edit(CourseViewModel course)
        {
            // Athuga hvort input sé valid
            if (ModelState.IsValid)
            {
                // Uppfæra upplýsingar í course
                if (service.updateCourse(course))
                    return RedirectToAction("List");
            }
            // Ef input ekki valid, sýna view aftur
            return View("AdminTeacherViews/edit", course);
        }

        /// <summary>
        /// Síða sem biður um staðfestingu á því hvort eigi að eyða Course með ID 'id'.
        /// Fer á error síðu ef Course er ekki til, eða 'id' er null.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult remove(int? id)
        {
            // Athuga hvort id sé null
            if (id.HasValue)
            {
                // Finna Course sem á að eyða
                var toRemove = service.getCourseById(id);

                // Ef Course er til birta staðfestingar view, annars error.
                if (toRemove != null)
                    return View("AdminTeacherViews/remove", toRemove);
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Eyðir Course með ID 'toRemove.id' úr gagnagrunni.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult remove(CourseViewModel toRemove)
        {
            // toRemove eytt úr DB, ef eitthvað mistekst birtist error view
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

        /// <summary>
        /// Sýnir Error view.
        /// </summary>
        /// <returns></returns>
        public ActionResult error() { return View(); }
    }
}