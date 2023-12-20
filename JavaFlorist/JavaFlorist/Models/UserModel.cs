﻿using System;
using System.ComponentModel.DataAnnotations;

namespace JavaFlorist.Models
{
	public class UserModel
	{
		[Key]
		public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime BirthDay { get; set; }
        public int Gender { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string? Avatar { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime Create_at { get; set; }
        public DateTime Update_at { get; set; }
        public virtual ICollection<OrderModel> OrderModels { get; set; } = new List<OrderModel>();
        public virtual ICollection<RateModel> Rates { get; set; } = new List<RateModel>();
    }
}

