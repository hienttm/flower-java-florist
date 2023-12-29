using System;
using System.ComponentModel.DataAnnotations;

namespace JavaFlorist.Models
{
	public class OrderModel
	{
        [Key]

		public int Id { get; set; }
        public int UserId { get; set; }

        public DateTime DateOrder { get; set; }

        public DateTime? DateNeed { get; set; }

        public DateTime? DateShip { get; set; }

        public string? UserName { get; set; }

        public string Address { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string HowToPay { get; set; } = null!;

        public string ShipingWay { get; set; } = null!;

        public double TransportFee { get; set; }

        public int Status { get; set; }

        public string? Note { get; set; }

        public virtual ICollection<OrderDetailModel> OrderDetails { get; set; } = new List<OrderDetailModel>();

        public UserModel Users { get; set; } = null!;
    }
}

