using MooshakV2.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MooshakV2.ViewModels
{
    public class SubmissionViewModel
    {
        [Display(Name = "Success")]
        public int success { get; set; }

        [Display(Name = "Date")]
        public DateTime date { get; set; }

        [Display(Name = "Count")]
        public int count { get; set; }

        public int id { get; set; }

        public int assignmentId { get; set; }

        public int partId { get; set; }

        public string userId { get; set; }

        public string filename { get; set; }

        public string mime { get; set; }

        [Required(ErrorMessage = "You have to select file")]
        [FileExtensions(Extensions = ".cpp, .cbp, .h", ErrorMessage = "Please select a valid file type")]
        public HttpPostedFileBase file { get; set; }

    }
}