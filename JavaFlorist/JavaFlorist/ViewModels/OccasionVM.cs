using System;
using System.ComponentModel.DataAnnotations;

namespace JavaFlorist.ViewModels
{
	public class OccasionVM
	{
		public int Id { set; get; }
        [Required(ErrorMessage = "Please input Name Occasion")]
        public string Name { set; get; }
        [Required(ErrorMessage = "Please input Slug Occasion")]
        public string Slug { set; get; }
		public int Status { set; get; }
	}
}

