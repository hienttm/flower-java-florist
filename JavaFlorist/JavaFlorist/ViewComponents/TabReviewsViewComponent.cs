using System;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.ViewComponents
{
	public class TabReviewsViewComponent:ViewComponent
	{
		private readonly DataContext _context;
		public TabReviewsViewComponent(DataContext context)
		{
			_context = context;
		}

        public IViewComponentResult Invoke()
		{
            string slug = Request.Query["Slug"];
			var product = _context.Products.Include(p => p.Occasion).Where(p => p.Slug == slug).FirstOrDefault();
            var data = _context.Rates.Include(p => p.User).Include(p => p.Product).Where(p => p.ProductId == product.Id).AsQueryable();
			var result = data.Select(r => new TabRatesViewModel
			{
				Id = r.Id,
				NameUser = r.User.Username,
				Avatar = r.User.Avatar,
				Comment = r.Comment,
				Star = r.Star,
				Time = r.UpdateTime.ToString(("MMMM d, yyyy"))
			});
			return View(result);
		}

    }
}

