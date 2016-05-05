using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mooshak2_FirstTest_KR.Models.ViewModels
{
    public class AssignmentViewModel
    {
        public int id { get; set; }
        public int courseId { get; set; }

        [Required(ErrorMessage = "The assignment must have a title!")]
        public string title { get; set; }
        
        public string description { get; set; }

        [Required(ErrorMessage = "The assignment must have some weight!")]
        public int weight { get; set; }

        // TODO: setja requirement á min stærð þ.a. það sé alltaf assignmentPart?
        public List<AssignmentPartViewModel> assignmentParts { get; set; }
    }
}