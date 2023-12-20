using System;
using System.ComponentModel.DataAnnotations;

namespace JavaFlorist.Models
{
	public class BannerContentModel
	{
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please input name Banner")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please input name Slug")]
        public string Slug { get; set; }
        public int Order { get; set; }
        public int Status { get; set; }
    }
}

