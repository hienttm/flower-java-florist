using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JavaFlorist.Models
{
	public class RateModel
	{
        [Key, ForeignKey("UserModel")]
		public int Id { set; get; }
		public int ProductId { set; get; }
		public ProductModel Product { set; get; }
		public int UserId { set; get; }
        public UserModel User { get; set; } = null!;
		public string Comment { set; get; }
		public int Star { set; get; } = 0;
    }
}

