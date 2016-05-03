using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mooshak2_FirstTest_KR.Models;
using Mooshak2_FirstTest_KR.Models.ViewModels;

namespace Mooshak2_FirstTest_KR.Services
{
    public class AssignmentService
    {
        private ApplicationDbContext database;

        public AssignmentService() { database = new ApplicationDbContext(); }

        public List<AssignmentViewModel> getAllAssignments() { return new List<AssignmentViewModel>(); }

        public List<AssignmentViewModel> getAllAssignmentsInCourse(int courseId)
        {
            return new List<AssignmentViewModel>();
        }

        public List<AssignmentViewModel> getAssignmentInCourse(int courseId, int assignmentId)
        {
            return new List<AssignmentViewModel>();
        }
    }
}