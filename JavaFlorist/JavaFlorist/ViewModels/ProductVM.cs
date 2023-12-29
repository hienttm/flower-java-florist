using System;
using JavaFlorist.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JavaFlorist.ViewModels
{
	public class ProductVM
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Thumb { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Content { get; set; }
        public OccasionModel Occasion { get; set; }
        public DiscountVM Discount { get; set; }
        [NotMapped]
        [FileExtensions]
        public IFormFile ImageUpload { get; set; }
        public int OccasionId { get; set; }
        public int? DiscountId { get; set; }
        public int Status { get; set; }
    }
}

