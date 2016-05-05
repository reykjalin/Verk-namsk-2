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

        public ActionResult Index() { return RedirectToAction("List"); }
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
        public ActionResult edit()
        {
            return View();
        }

        //Remove assignment
        public ActionResult remove()
        {
            return View();
        }

        //Get details about the given id
        public ActionResult details(int id)
        {
            return View();
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