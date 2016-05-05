using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MooshakV2.ViewModels
{
    public class AssignmentPartViewModel
    {
        private string title { get; set; }
        [Required(ErrorMessage="You must write a description!")]
        private string description { get; set; }
        private int weight { get; set; }
        private int id { get; set; }
    }
}