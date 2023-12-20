using System;
using System.ComponentModel.DataAnnotations;

namespace JavaFlorist.Models
{
	public class OrderStatusModel
	{
        [Key]
        public int Id { get; set; }

        public string NameStatus { get; set; } = null!;

        public string? Detail { get; set; }

        public virtual ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();
    }
}

