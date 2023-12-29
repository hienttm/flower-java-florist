using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JavaFlorist.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class HomeController:Controller
	{
		public IActionResult Index()
		{
			if (User.IsInRole("admin"))
			{
                return View();
            }
			else
			{
				return RedirectToAction("Index", "Login");
			}
		}
	}
}

