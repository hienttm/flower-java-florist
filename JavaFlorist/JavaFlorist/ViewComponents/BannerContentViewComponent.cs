using System;
using JavaFlorist.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.ViewComponents
{
	public class BannerContentViewComponent:ViewComponent
	{
		private readonly DataContext _context;
		public BannerContentViewComponent(DataContext context)
		{
			_context = context;
		}
        public IViewComponentResult Invoke()
		{
			var data = _context.BannerContents.Where(p => p.Status == 1).Include(p => p.Product).Where(p => p.Product.Status == 1).Where(p => p.ProductId == p.Product.Id).FirstOrDefault();
			return View(data);
		}

    }
}

