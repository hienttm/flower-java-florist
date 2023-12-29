using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace JavaFlorist.ViewModels
{
	public class UserViewModel
	{
        public int Id { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Plase Input UserName")]
        [MaxLength(30, ErrorMessage = "Max value 20 key")]
        public string Username { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please Input Email")]
        [EmailAddress(ErrorMessage = "Not in correct format")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please input password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Sex")]
        [Required(ErrorMessage = "Please Input Sex")]
        public int Gender { get; set; }

        [Display(Name = "Phone Number")]
        [MaxLength(24, ErrorMessage = "Max 24 Key")]
        [RegularExpression(@"0[329875]\d{8}", ErrorMessage = "Not in correct format number phone VietNam")]
        public string Phone { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Please input Address")]
        public string Address { get; set; }

        [Display(Name = "Avatar")]
        public string Avatar { get; set; }

        [NotMapped]
        [FileExtensions]
        public IFormFile ImageUpload { get; set; }

        public string Roles { get; set; }


        [Display(Name = "ReapeatPassword")]
        [Required(ErrorMessage = "Please input ReapeatPassword")]
        [DataType(DataType.Password)]
        public string? ReapeatPassword { get; set; }
       
    }
}

