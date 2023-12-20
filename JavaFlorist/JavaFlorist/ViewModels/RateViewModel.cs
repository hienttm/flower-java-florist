using System;
namespace JavaFlorist.ViewModels
{
	public class RateViewModel
	{
		public int Id { set; get; }
		public int ProductId { set; get; }
		public int UserId { set; get; }
		public int Star { set; get; }
		public string? Comment { set; get; }
	}
}

