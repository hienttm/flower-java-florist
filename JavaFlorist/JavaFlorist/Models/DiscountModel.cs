using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace JavaFlorist.Models
{
	public class DiscountModel
	{
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please input discount")]
        [Column(TypeName = "decimal(14,2)")]
        public decimal Discount { get; set; }

        public ICollection<ProductModel> ProductModels { get; set; }
    }
}

