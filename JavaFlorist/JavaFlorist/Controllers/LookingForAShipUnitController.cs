using System;
using JavaFlorist.Helpers;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Controllers
{
	public class LookingForAShipUnitController:Controller
	{
		private readonly DataContext _context;
		public LookingForAShipUnitController(DataContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
            if (User.Identity.IsAuthenticated)
            {
                var customerID = int.Parse(@User.FindFirst(MySetting.CLAIM_CUSTOMERID)?.Value);
                var user = _context.Users.SingleOrDefault(u => u.Id == customerID);
                var order = _context.OrderDetails.Include(o => o.Order).Where(p => p.OrderStatusId == 2)
                    .Include(p => p.Product).Where(u => u.Order.UserId == user.Id);
                var result = order.Select(p => new CustomerOrderViewModel
                {
                    NameProduct = p.Product.Name,
                    ImageProduct = p.Product.Thumb,
                    Price = p.Discount,
                    Quantity = p.Quantity,
                    Total = p.Discount * p.Quantity,
                });
                return View(result);
            }
            return View();
		}
	}
}

