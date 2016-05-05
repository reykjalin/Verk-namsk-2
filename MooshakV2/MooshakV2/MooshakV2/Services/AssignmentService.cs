using MooshakV2.DAL;
using MooshakV2.Models;
using MooshakV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakV2.Services
{
    public class AssignmentService
    {
        private DatabaseDataContext contextDb;
        public AssignmentService()
        {
            contextDb = new DatabaseDataContext();
        }

        public List<AssignmentViewModel> getAllAssignments()
        {
            var assignmentList = (from alist in contextDb.assignments
                                  select alist).ToList();

            var assignmentModelList = new List<AssignmentViewModel>();
            foreach(var assignment in assignmentList)
            {
                var viewModel = new AssignmentViewModel();
                viewModel.title = assignment.title;
                viewModel.description = assignment.description;
                viewModel.weight = assignment.weight;
                viewModel.id = assignment.id;
                viewModel.courseId = assignment.courseId;
                assignmentModelList.Add(viewModel);
            }
            return assignmentModelList;
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

        public bool addAssignment(AssignmentViewModel newAssignmentModel)
        {
            Assignment newAssignment = new Assignment();
            newAssignment.title = newAssignmentModel.title;
            newAssignment.description = newAssignmentModel.description;
            newAssignment.weight = newAssignmentModel.weight;
            newAssignment.courseId = newAssignmentModel.courseId;

            contextDb.assignments.Add(newAssignment);
            contextDb.SaveChanges();

            return true;
             


        }
    }
}