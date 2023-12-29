using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JavaFlorist.Models
{
	public class ProductModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
		public string Thumb { get; set; }
		public string Description { get; set; }
        [Required(ErrorMessage = "Please input price")]
        [Column(TypeName = "decimal(14,2)")]
        public decimal Price { get; set; }
        public string Content { get; set; }
		public int OccasionId { get; set; }
		public OccasionModel Occasion { get; set; }
		public int? DiscountId { get; set; }
		public DiscountModel Discount { get; set; }
		public int CountBuy { get; set; } = 0;
		public int Status { get; set; }
        [NotMapped]
        [FileExtensions]
        public IFormFile ImageUpload { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Update_at { get; set; }
        public virtual ICollection<OrderDetailModel> OrderDetails { get; set; } = new List<OrderDetailModel>();
        public virtual ICollection<RateModel> Rates { get; set; } = new List<RateModel>();
        public virtual ICollection<BannerContentModel> BannerContentModels { get; set; }
    }
}

