using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MooshakV2.ViewModels
{
    public class AssignmentPartViewModel
    {
        public string title { get; set; }
        [Required(ErrorMessage="You must write a description!")]
        public string description { get; set; }
        public int weight { get; set; }
        public int id { get; set; }
    }
}