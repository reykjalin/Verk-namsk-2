using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MooshakV2.ViewModels
{
    public class FileUploadViewModel
    {
        [Display(Name = "File")]
        [Required(ErrorMessage = "You have to select file")]
        ////[FileExtensions(Extensions = "cpp", ErrorMessage = "Please select a valid file type")]
        public HttpPostedFileBase file { get; set; }

        public int? assignmentId { get; set; }

        public int partId { get; set; }

        public int success { get; set; }

        public int count { get; set; }
    }
}