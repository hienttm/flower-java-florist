using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace JavaFlorist.ViewModels
{
	public class UserViewModel
	{
        public int Id { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "*")]
        [MaxLength(20,ErrorMessage = "Max value 20 key")]
        public string Firstname { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "*")]
        [MaxLength(20, ErrorMessage = "Max value 20 key")]
        public string Lastname { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "*")]
        [MaxLength(30, ErrorMessage = "Max value 20 key")]
        public string Username { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Not in correct format")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "Birth Day")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }

        [Display(Name = "Sex")]
        [Required(ErrorMessage = "*")]
        public int Gender { get; set; }

        [Display(Name = "Phone Number")]
        [MaxLength(24, ErrorMessage = "Max 24 Key")]
        [RegularExpression(@"0[9875]\d{8}", ErrorMessage = "Not in correct format number phone VietNam")]
        public string Phone { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "*")]
        public string Address { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "*")]
        public string City { get; set; }

        [Display(Name = "Avatar")]
        public string Avatar { get; set; }
    }
}

