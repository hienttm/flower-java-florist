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

        public virtual ICollection<OrderDetailModel> OrderDetails { get; set; } = new List<OrderDetailModel>();
    }
}

