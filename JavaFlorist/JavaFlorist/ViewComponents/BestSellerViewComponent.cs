using System;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.ViewComponents
{
	public class BestSellerViewComponent:ViewComponent
	{
		private readonly DataContext _context;
		public BestSellerViewComponent(DataContext context)
		{
			_context = context;
		}
		public IViewComponentResult Invoke()
		{
			var data = _context.Products.Include(d => d.Rates).Where(d => d.Status == 1).OrderByDescending(d => d.CountBuy).Take(6);
			var result = data.Select(r => new FeaturedViewModel
			{
                Id = r.Id,
                Name = r.Name,
                Image = r.Thumb,
                Slug = r.Slug,
                Rate = r.Rates != null && r.Rates.Any() ? Math.Round((r.Rates.Average(p => p.Star)), 1) : 0,
                Price = r.Price,
                Discount = decimal.Parse(((r.Price * (100 - r.Discount.Discount)) / 100).ToString("0.00")),
            });
			return View(result);
		}

    }
}

