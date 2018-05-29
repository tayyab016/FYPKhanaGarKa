using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models.ViewModels
{
    public class ModifyPasswordViewModel
    {
        [Required(ErrorMessage ="Your old password is required")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Your New password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum length should be 6 charachters")]
        [MaxLength(10, ErrorMessage = "Maximum length should be 10 charachters")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Your old password is required")]
        [DataType(DataType.Password),MinLength(6),MaxLength(10)]
        [Compare("NewPassword",ErrorMessage ="Confirm Password does not match with password.")]
        public string ConfirmPassword { get; set; }
    }
}
