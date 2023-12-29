using System;
using JavaFlorist.Helpers;
using System.ComponentModel.DataAnnotations;

namespace JavaFlorist.ViewModels
{
	public class ChangePassword
	{
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please input password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "ReapeatPassword")]
        [Required(ErrorMessage = "Please input ReapeatPassword")]
        [DataType(DataType.Password)]
        public string ReapeatPassword { get; set; }
    }
}

