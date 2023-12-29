using System;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JavaFlorist.ViewComponents
{
	public class SidebarViewComponent : ViewComponent
	{
		private readonly DataContext _context;
		public SidebarViewComponent(DataContext context)
		{
			_context = context;
		}
		public IViewComponentResult Invoke()
		{
            var data = _context.Occasions.AsQueryable().Where(d => d.ProductModels.Any(p => p.Status == 1));
			var result = data.Select(r => new SidebarViewModel
			{
				Id = r.Id,
				Name = r.Name,
				Count = r.ProductModels.Count
			});
            return View(result);
		}
	}
}

