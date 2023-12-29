using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JavaFlorist.Areas.Manager.Controllers
{
	[Area("Manager")]
	[Authorize]
	public class HomeController:Controller
	{
		public IActionResult Index()
		{
			if (User.IsInRole("manager"))
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

