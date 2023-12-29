using System;
using JavaFlorist.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JavaFlorist.ViewComponents
{
	public class ListOrderViewComponent:ViewComponent
	{
		private readonly DataContext _context;
		public ListOrderViewComponent(DataContext context)
		{
			_context = context;
		}
        public IViewComponentResult Invoke()
		{
            var data = _context.OrderStatuses.ToList();
			return View(data);
		}

    }
}

