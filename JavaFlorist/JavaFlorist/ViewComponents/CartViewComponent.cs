using System;
using System.Collections.Generic;
using JavaFlorist.Helpers;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JavaFlorist.ViewComponents
{
	public class CartViewComponent:ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			var cart = HttpContext.Session.Get <List<CartViewModel>>(MySetting.Cart_Key)?? new List<CartViewModel>();

			return View("CartPanel",new CartsViewModel
			{
				Quantity = cart.Count(),
				Total = cart.Sum(p => p.Total)
			});
		}
	}
}

