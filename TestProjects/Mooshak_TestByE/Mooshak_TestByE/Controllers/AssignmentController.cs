using Mooshak_TestByE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooshak_TestByE.Controllers
{
    public class AssignmentController : Controller
    {
        private AssignmentService service;
        public AssignmentController()
        {
            service = new AssignmentService();
        }
        // Create assignment
        public ActionResult create()
        {
            return View();
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