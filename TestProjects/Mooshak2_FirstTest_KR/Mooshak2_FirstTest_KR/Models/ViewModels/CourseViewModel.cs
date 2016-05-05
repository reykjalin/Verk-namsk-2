using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace Mooshak2_FirstTest_KR.Models.ViewModels
{
    public class CourseViewModel
    {
        public int id { get; set; }

        [Required(ErrorMessage = "You must specify a name!")]
        public string title { get; set; }

        [Required(ErrorMessage = "You must write a description!")]
        public string description { get; set; }
    }
}