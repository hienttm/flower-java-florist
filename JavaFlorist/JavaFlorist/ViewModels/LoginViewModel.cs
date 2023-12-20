using System;
using System.ComponentModel.DataAnnotations;

namespace JavaFlorist.ViewModels
{
	public class LoginViewModel
	{
        [Display(Name = "Email")]
        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Not in correct format")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

