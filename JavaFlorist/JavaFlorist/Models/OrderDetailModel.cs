using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace JavaFlorist.Models
{
	public class OrderDetailModel
	{
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        [Column(TypeName = "decimal(14,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal Discount { get; set; }

        public int OrderStatusId { get; set; } = 1;
        public OrderStatusModel OrderStatus { get; set; } = null!;

        public OrderModel Order { get; set; } = null!;

        public ProductModel Product { get; set; } = null!;
    }
}

