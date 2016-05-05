using Mooshak_TestByE.Models;
using Mooshak_TestByE.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mooshak_TestByE.Services
{
    public class AssignmentService
    {
        private ApplicationDbContext database;
        public AssignmentService()
        {
            database = new ApplicationDbContext();
        }

        public List<AssignmentViewModel> getAllAssignments()
        {
            return new List<AssignmentViewModel>();
        }

        public List<AssignmentViewModel> getAllAssignmentsInCourse(int courseId)
        {
            return new List<AssignmentViewModel>();
        }

        public List<AssignmentViewModel> getAssignmentInCourse(int courseId, int assignmentId)
        {
            return new List<AssignmentViewModel>();
        }

        public bool handInAssignment(int assignmentId, int courseId, string data)
        {
            return true;
        }
    }
}