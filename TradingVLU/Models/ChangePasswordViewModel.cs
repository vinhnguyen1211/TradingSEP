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
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{4,20}$", ErrorMessage = "Minimum four characters and maximun twenty characters, at least one uppercase letter, one lowercase letter and one number.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Compare("NewPassword")]
        [Display(Name = "Confirm password")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{4,20}$", ErrorMessage = "Minimum four characters and maximun twenty characters, at least one uppercase letter, one lowercase letter and one number.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string ReConfirmPassword { get; set; }
    }
}