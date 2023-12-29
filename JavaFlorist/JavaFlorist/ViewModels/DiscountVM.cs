using System;
using System.ComponentModel.DataAnnotations;

namespace JavaFlorist.ViewModels
{
	public class DiscountVM
	{
		public int Id { set; get; }
		[Required(ErrorMessage="Please input % Discount")]
        public decimal Discount { get; set; }
    }
}

