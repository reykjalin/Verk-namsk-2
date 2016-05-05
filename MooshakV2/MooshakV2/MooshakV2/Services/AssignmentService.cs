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
            foreach (var assignment in assignmentList)
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
			var assignmentEntities = (from assignments in contextDb.assignments
								  where courseId == assignments.courseId
								  select assignments).ToList();

			var assignmentList = new List<AssignmentViewModel>();

			foreach (var item in assignmentEntities)
			{
				var assignment = new AssignmentViewModel();

				assignment.title = item.title;
				assignment.description = item.description;
				assignment.id = item.id;
				assignment.weight = item.weight;
				assignment.courseId = item.courseId;

				assignmentList.Add(assignment);
			}

			return assignmentList;
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

        public AssignmentViewModel getAssignmentById(int? id)
        {
            var assignment = (from a in contextDb.assignments
                              where a.id == id
                              select a).FirstOrDefault();

            if (assignment == null)
                return null;

            var model = new AssignmentViewModel();
            model.title = assignment.title;
            model.description = assignment.description;
            model.weight = assignment.weight;
            model.id = assignment.id;

            return model;
        }

        public bool updateAssignment(AssignmentViewModel newData)
        {
            if (newData != null)
            {
                var oldAssignment = (from a in contextDb.assignments
                                     where a.id == newData.id
                                     select a).SingleOrDefault();

                if (oldAssignment != null)
                {
                    oldAssignment.title = newData.title;
                    oldAssignment.description = newData.description;
                    oldAssignment.weight = newData.weight;
                    oldAssignment.id = newData.id;
                    oldAssignment.courseId = newData.courseId;

                    contextDb.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool removeAssignment(int id)
        {
            contextDb.assignments.Remove((from a in contextDb.assignments
                                          where a.id == id
                                          select a).SingleOrDefault());

            contextDb.SaveChanges();
            return true;
        }
    }
}