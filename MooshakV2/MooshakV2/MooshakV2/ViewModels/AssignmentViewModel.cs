using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MooshakV2.ViewModels
{
    public class AssignmentViewModel
    {
        [Required(ErrorMessage="You must specify a name")]
        private string title { get; set; }
        private string description { get; set; }
        private int weight { get; set; }
        //private List<AssignmentPartViewModel> assignmentParts;
    }
}