using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MooshakV2.ViewModels
{
    public class UserViewModel
    {
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "A user must have a user name!")]
        public string userName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "You must enter an e-mail address!")]
        [EmailAddress]
        public string email { get; set; }

        [Display(Name = "User Role")]
        [Required(ErrorMessage = "You must define a role for this user!")]
        public string roleName { get; set; }
    }
}