using JT_TestProject.Models;
using JT_TestProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JT_TestProject.Services
{
	public class AssignmentsServices
	{
		private ApplicationDbContext _db;

		public AssignmentsServices()
		{
			_db = new ApplicationDbContext();
		}
		public List<AssignmentViewModel> GetAssignmentsInCourse(int courseID)
		{
			// TODO: 
			return null;
		}
		public AssignmentViewModel GetAssignmentByID(int assignmentID)
		{
			var assignment = _db.Assignments.SingleOrDefault(x => x.ID == assignmentID);
			if(assignment == null)
			{
				//TODO: kasta villu
			}

			var milestones = _db.Milestones
				.Where(x => x.AssignmentID == assignmentID)
				.Select(x => new AssignmentMilestoneViewModel
				{
					Title = x.Title
				})
				.ToList();

			var viewModel = new AssignmentViewModel
			{
				Title = assignment.Title,
				Milestone = milestones
			};

			return viewModel;
		}
	}
}