using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace JavaFlorist.ViewModels
{
	public class ProductDetailViewModel
	{
		public int Id { set; get; }
		public string Name { set; get; }
		public string Image { set; get; }
		public string Slug { get; set; }
		public string Occasion { set; get; }
		public decimal Price { set; get; }
        [Column(TypeName = "decimal(14,2)")]
        public decimal Discount { get; set; }
        public string Description { set; get; }
		public string Content { set; get; }

	}
}

