using System;
using System.ComponentModel.DataAnnotations;

namespace JavaFlorist.Models
{
	public class OccasionModel
	{
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please input name Occasion")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please input slug Occasion")]
        public string Slug { get; set; }
        public int Status { get; set; }
        public ICollection<ProductModel> ProductModels { get; set; }
    }
}

