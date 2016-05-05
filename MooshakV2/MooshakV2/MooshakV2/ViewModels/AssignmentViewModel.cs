using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MooshakV2.ViewModels
{
    public class AssignmentViewModel
    {
        [Required(ErrorMessage="You must specify a name!")]
        public string title { get; set; }
        public string description { get; set; }
        [Required(ErrorMessage="The assignment must have some weight")]
        public int weight { get; set; }
        public int id { get; set; }
        [Required(ErrorMessage="You have to select course!")]
        public int courseId { get; set; }
        public List<AssignmentPartViewModel> assignmentParts { get; set; }
    }
}