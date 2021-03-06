﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MooshakV2.ViewModels
{
    public class AssignmentViewModel
    {
        private List<AssignmentPartViewModel> parts;

        [Display(Name = "Title")]
        [Required(ErrorMessage="You must specify a name!")]
        public string title { get; set; }

        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "Weight")]
        [Required(ErrorMessage="The assignment must have some weight")]
        public int weight { get; set; }
        
        public int id { get; set; }

        [Display(Name = "Course")]
        [Required(ErrorMessage="You have to select course!")]
        public int courseId { get; set; }

        [Display(Name = "Input")]
        [Required(ErrorMessage = "There must be an input!")]
        public string input { get; set; }

        [Display(Name = "Output")]
        [Required(ErrorMessage = "There must be an output!")]
        public string output { get; set; }

        [Display(Name = "Date and Time")]
        [Required(ErrorMessage = "There must be a final submission date and time")]
        public DateTime? date { get; set; }

        public List<AssignmentPartViewModel> assignmentParts
        {
            get
            {
                if (parts == null)
                    parts = new List<AssignmentPartViewModel>();
                return parts;
            }
            set { parts = value; }
        }

        // Á ekki að vera skv. E
        //[Display(Name = "File")]
        //[Required(ErrorMessage ="You have to select file")]
        //[FileExtensions(Extensions =".cpp,.cbp,.h", ErrorMessage ="Please select a valid file type")]
        //public HttpPostedFileBase file { get; set; }
    }
}