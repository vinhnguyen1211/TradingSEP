using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TradingVLU.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Compare("NewPassword")]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string ReConfirmPassword { get; set; }
    }
}