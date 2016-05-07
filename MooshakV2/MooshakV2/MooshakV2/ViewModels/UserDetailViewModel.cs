using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MooshakV2.ViewModels
{
    public class UserDetailViewModel
    {
        public UserViewModel userModel { get; set; }

        [Required(ErrorMessage = "The user must have a name!")]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Required(ErrorMessage = "The user must have a SSN!")]
        [Display(Name = "SSN")]
        public string ssn { get; set; }
    }
}