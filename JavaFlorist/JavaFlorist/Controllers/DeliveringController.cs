using System;
using JavaFlorist.Helpers;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Controllers
{
	public class DeliveringController:Controller
	{
		private readonly DataContext _context;
		public DeliveringController(DataContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
            var customerID = int.Parse(@User.FindFirst(MySetting.CLAIM_CUSTOMERID)?.Value);
            var user = _context.Users.SingleOrDefault(u => u.Id == customerID);
            var order = _context.OrderDetails.Include(o => o.Order).Where(p => p.OrderStatusId == 3)
                .Include(p => p.Product).Where(u => u.Order.UserId == user.Id);
            var result = order.Select(p => new CustomerOrderViewModel
            {
                Id = p.Id,
                NameProduct = p.Product.Name,
                ImageProduct = p.Product.Thumb,
                Price = p.Discount,
                Quantity = p.Quantity,
                Total = p.Discount * p.Quantity,
            });
            return View(result);
		}

        public IActionResult GoodsReceived(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var customerID = int.Parse(@User.FindFirst(MySetting.CLAIM_CUSTOMERID)?.Value);
                var user = _context.Users.SingleOrDefault(u => u.Id == customerID);
                var order = _context.OrderDetails.Where(p => p.Id == Id).Where(u => u.Order.UserId == user.Id).FirstOrDefault();
                order.OrderStatusId = 4;
                _context.Update(order);
                _context.SaveChanges();
                return RedirectToAction("Index", "Complete");
            }
            return View();
        }

    }
}

