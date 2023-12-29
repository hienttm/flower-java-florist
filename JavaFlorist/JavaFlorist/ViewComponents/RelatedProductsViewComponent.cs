using System;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.ViewComponents
{
	public class RelatedProductsViewComponent : ViewComponent
	{
		private readonly DataContext _context;
		public RelatedProductsViewComponent(DataContext context)
		{
			_context = context;
		}
		public IViewComponentResult Invoke()
		{
            string slug = Request.Query["Slug"];
			var product = _context.Products.Include(p => p.Occasion).Where(p => p.Slug == slug).FirstOrDefault();
			var data = _context.Products.Include(p => p.Occasion).Include(p => p.Discount).Where(p => p.OccasionId == product.OccasionId);
			var result = data.Select(r => new ProductViewModel
			{
				Id = r.Id,
				Name = r.Name,
				Slug = r.Slug,
				Price = r.Price,
                Discount = decimal.Parse(((r.Price * (100 - r.Discount.Discount)) / 100).ToString("0.00")),
				Image = r.Thumb,
				Occasion = r.Occasion.Name
            });
			return View(result);
		}
	}
}


