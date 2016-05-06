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
        public List<UserViewModel> studentList { get; set; }
        public List<UserViewModel> taList { get; set; }
        public List<UserViewModel> teacherList { get; set; }
	}
}