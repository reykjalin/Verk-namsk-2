using MooshakV2.Services;
using MooshakV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MooshakV2.Controllers
{
    public class AssignmentController : Controller
    {
        private AssignmentService service;
        private CourseService courseService;
             
        public AssignmentController()
        {
            service = new AssignmentService();
            courseService = new CourseService();
        }

        public ActionResult Index() { return RedirectToAction("List"); }
        // Create assignment
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult create()
        {
            prepareDropdown();
            return View("AdminTeacherViews/create");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult create(AssignmentViewModel newAssignment)
        {
            ModelState["id"].Errors.Clear();
            if (ModelState.IsValid)
            {
                AssignmentViewModel assignment = new AssignmentViewModel();
                assignment.courseId = newAssignment.courseId;
                if (service.addAssignment(newAssignment))
                    return RedirectToAction("List");
            }

            return View("AdminTeacherViews/create", newAssignment);

        }

        [HttpGet]
        [Authorize]
        public ActionResult list()
        {
            var model = service.getAllAssignments();
            if(User.IsInRole("Student"))
                return View("StudentViews/list", model);

            return View("AdminTeacherViews/list", model);
        }

        //Change assignment
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult edit(int? id)
        {
            if (id.HasValue)
            {
                var model = service.getAssignmentById(id);

                if (model != null)
                {
                    prepareDropdown();
                    return View("AdminTeacherViews/edit", model);
                }
            
            }
            return RedirectToAction("Error");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult edit(AssignmentViewModel assignment)
        {
            if (ModelState.IsValid)
            {
                if (service.updateAssignment(assignment))
                    return RedirectToAction("List");
            }

            return View("AdminTeacherViews/edit", assignment);
        }

        //Remove assignment
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult remove(int? id)
        {
            if(id.HasValue)
            {
                var toRemove = service.getAssignmentById(id);
                if (toRemove != null)
                    return View("AdminTeacherViews/remove", toRemove);
            }
            return RedirectToAction("Error");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult remove(AssignmentViewModel toRemove)
        {
            if (service.removeAssignment(toRemove.id))
                return RedirectToAction("List");

            return RedirectToAction("Error");

        }

        //Get details about the given id
        [HttpGet]
        [Authorize]
        public ActionResult details(int? id)
        {
            if(id.HasValue)
            {
                var model = service.getAssignmentById(id);
                if(User.IsInRole("Student"))
                    return View("StudentViews/details", model);

                return View("AdminTeacherViews/details", model);
            }
            return RedirectToAction("Error");
        }

        public ActionResult error() { return View(); }

        //Get list of assignments in a course
        public ActionResult allCourseAssignments(int courseId)
        {
            return View();
        }

        //Get an assignment in a course
        public ActionResult courseAssignment(int courseId, int assignmentId)
        {
            return View();
        }

        //Hand in an assignment
        public ActionResult handInAssignment(int assignmentId, int userId, string data)
        {
            return View();
        }

        private void prepareDropdown()
        {
            var courseList = courseService.getAllCourses();
            List<SelectListItem> courseDropDown = new List<SelectListItem>();

            foreach (var item in courseList)
                courseDropDown.Add(new SelectListItem { Text = item.title, Value = item.id.ToString() });

            ViewData["Courselist"] = courseDropDown;
        }
    }
}