using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MooshakV2.ViewModels
{
    public class AssignmentCourseViewModel
    {
        public AssignmentViewModel assignmentInfo { get; set; }
        public List<SelectListItem> courseList { get; set; }
    }
}