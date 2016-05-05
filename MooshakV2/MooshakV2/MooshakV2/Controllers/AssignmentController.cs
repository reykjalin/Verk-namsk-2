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
        public AssignmentController()
        {
            service = new AssignmentService();
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        // Create assignment
        [HttpGet]
        public ActionResult create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult create(AssignmentViewModel newAssignment)
        {
            ModelState["id"].Errors.Clear();
            if (ModelState.IsValid)
            {
                if (service.addAssignment(newAssignment))
                    return RedirectToAction("List");
            }

            return View(newAssignment);

        }

        [HttpGet]
        public ActionResult list()
        {
            var model = service.getAllAssignments();
            return View(model);
        }

        //Change assignment
        [HttpGet]
        public ActionResult edit(int? id)
        {
            if (id.HasValue)
            {
                var model = service.getAssignmentById(id);

                if (model != null)
                    return View(model);
            }
            return RedirectToAction("Error");
        }

        [HttpPost]
        public ActionResult edit(AssignmentViewModel assignment)
        {
            if (ModelState.IsValid)
            {
                if (service.updateAssignment(assignment))
                    return RedirectToAction("List");
            }

            return View(assignment);
        }

        //Remove assignment
        [HttpGet]
        public ActionResult remove(int? id)
        {
            if(id.HasValue)
            {
                var toRemove = service.getAssignmentById(id);
                if (toRemove != null)
                    return View(toRemove);
            }
            return RedirectToAction("Error");
        }

        [HttpPost]
        public ActionResult remove(AssignmentViewModel toRemove)
        {
            if (service.removeAssignment(toRemove.id))
                return RedirectToAction("List");

            return RedirectToAction("Error");

        }

        //Get details about the given id
        [HttpGet]
        public ActionResult details(int? id)
        {
            if(id.HasValue)
            {
                var model = service.getAssignmentById(id);
                return View(model);
            }
            return RedirectToAction("Error");
        }

        //Get list of assignments
        public ActionResult assignments()
        {
            return View();
        }

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


    }
}