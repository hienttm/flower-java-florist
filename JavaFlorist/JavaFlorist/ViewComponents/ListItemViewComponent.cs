using System;
using JavaFlorist.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.ViewComponents
{
	public class ListItemViewComponent:ViewComponent
	{
		private readonly DataContext _context;
		public ListItemViewComponent(DataContext context)
		{
			_context = context;
		}
		public IViewComponentResult Invoke()
		{
			var product = _context.Products.Include(p => p.Discount);
			var Data = _context.Occasions.Include(d => d.ProductModels).ThenInclude(p => p.Discount).Where(d => d.Status == 1 && d.ProductModels.Any(p => p.Status == 1));
			return View(Data);
		}
	}
}

