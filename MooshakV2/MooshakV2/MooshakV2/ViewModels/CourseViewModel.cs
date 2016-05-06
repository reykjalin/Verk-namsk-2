using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MooshakV2.ViewModels
{
    /// <summary>
    /// ViewModel notað til að senda gögn á milli CourseService og 
    /// CourseController, og frá CourseController yfir í View
    /// </summary>
    public class CourseViewModel
    {
        public int id { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "You must specify a name!")]
        public string title { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "You must write a description!")]
        public string description { get; set; }
    }
}