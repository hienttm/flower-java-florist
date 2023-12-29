using System;
namespace JavaFlorist.ViewModels
{
	public class ListOrderVM
	{
		public	int Id { set; get; }
		public string NameUser { set; get; }
		public string Address { set; get; }
		public string HowToPay { set; get; }
		public string NameProduct { set; get; }
		public string Image { set; get; }
		public decimal Price { set; get; }
		public decimal Discount { set; get; }
		public string Note { set; get; }
		public int StatusOrder { set; get; }
	}
}

