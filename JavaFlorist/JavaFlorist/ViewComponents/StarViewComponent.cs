using System;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.ViewComponents
{
	public class StarViewComponent:ViewComponent
	{
		private readonly DataContext _context;
		public StarViewComponent(DataContext context)
		{
			_context = context;
		}
        public IViewComponentResult Invoke()
        {
            string slug = Request.Query["Slug"];
            var checkStar = _context.Rates.Include(p => p.Product).Where(p => p.Product.Slug == slug).Where(p => p.ProductId == p.Product.Id).Where(p => p.Star > 0).Count();
            if(checkStar > 0)
            {
            double averageStar = _context.Rates.Include(p => p.Product).Where(p => p.Product.Slug == slug).Where(p => p.ProductId == p.Product.Id).Average(p => p.Star);
            double roundedStar = Math.Round(averageStar, 1);
            var model = new StarRate
            {
                star = roundedStar
            };
            return View(model);
            }
            else
            {
                var model = new StarRate
                {
                    star = 0
                };
                return View(model);
            }

        }
    }
}

