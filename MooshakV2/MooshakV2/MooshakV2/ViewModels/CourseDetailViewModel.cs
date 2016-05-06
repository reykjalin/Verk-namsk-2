using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakV2.ViewModels
{
	public class CourseDetailViewModel
	{
		public CourseViewModel course { get; set; }
		public List<AssignmentViewModel> assignmentList { get; set; }
	}
}