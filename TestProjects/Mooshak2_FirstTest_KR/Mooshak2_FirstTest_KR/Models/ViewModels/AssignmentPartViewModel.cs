using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mooshak2_FirstTest_KR.Models.ViewModels
{
    public class AssignmentPartViewModel
    {
        public int id { get; set; }
        
        public int assignmentId { get; set; }

        [Required(ErrorMessage = "You must specify a description!")]
        public string description { get; set; }

        [Required(ErrorMessage = "You must specify this assignments weight!")]
        public int weight { get; set; }
    }
}