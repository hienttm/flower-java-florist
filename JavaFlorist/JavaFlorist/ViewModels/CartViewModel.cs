using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace JavaFlorist.ViewModels
{
	public class CartViewModel
	{
		public int Id { set; get; }
        public string Img { set; get; }
		public string Name { set; get; }
		public decimal Price { set; get; }
        [Column(TypeName = "decimal(14,2)")]
        public decimal Discount { get; set; }
        public int Quantity { set; get; }
		public decimal Total => Quantity * Discount;

	}
}

