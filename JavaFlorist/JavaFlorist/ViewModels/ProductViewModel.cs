using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace JavaFlorist.ViewModels
{
	public class ProductViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Slug { get; set; }
        [Column(TypeName = "decimal(14,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(14,2)")]
        public decimal Discount { get; set; }
		public string Image { get; set; }
		public string Occasion { get; set; }
	}
}

