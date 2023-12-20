using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace JavaFlorist.ViewModels
{
	public class CustomerOrderViewModel
	{
		public string NameProduct { get; set; }
		public string ImageProduct { get; set; }
		public int Quantity { get; set; }
        [Column(TypeName = "decimal(14,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(14,2)")]
        public decimal Total { get; set; }
	}
}

