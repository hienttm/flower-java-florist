using System;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JavaFlorist.ViewComponents
{
	public class HeroSliderViewComponent:ViewComponent
	{
		private readonly DataContext _context;
		public HeroSliderViewComponent(DataContext context)
		{
			_context = context;
		}
        public IViewComponentResult Invoke()
		{
			var data = _context.Banners.Where(p => p.Order == 2).Where(p => p.Status == 1);
			var result = data.Select(r => new HeroBannerViewModel
			{
				Id = r.Id,
				Name = r.Name,
				Slug = r.Slug,
				Image = r.Images,
				
			});
			return View(result);
		}

    }
}

