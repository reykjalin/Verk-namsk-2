using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JT_TestProject.Models.ViewModels
{
	public class AssignmentViewModel
	{
		public string Title { get; set; }

		public List<AssignmentMilestoneViewModel> Milestone { get; set; }
	}
}