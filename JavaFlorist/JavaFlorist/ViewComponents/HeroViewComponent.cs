using System;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JavaFlorist.ViewComponents
{
	public class HeroViewComponent:ViewComponent
	{
		private readonly DataContext _context;
		public HeroViewComponent(DataContext context)
		{
			_context = context;
		}
        public IViewComponentResult Invoke()
		{
			var data = _context.Banners.Where(p => p.Order == 1).Where(p => p.Status == 1).FirstOrDefault();
			var result = new HeroBannerViewModel
			{
				Id = data.Id,
				Name = data.Name,
				Image = data.Images,
				Slug = data.Slug
			};
			return View(result);
		}

    }
}

