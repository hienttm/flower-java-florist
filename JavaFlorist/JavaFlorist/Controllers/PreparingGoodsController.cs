using System;
using JavaFlorist.Helpers;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Controllers
{
	public class PreparingGoodsController:Controller
	{
		private readonly DataContext _context;
		public PreparingGoodsController(DataContext context)
		{
			_context = context;
		}
        [Authorize]
        [HttpGet]
		public IActionResult Index()
		{
            if (User.Identity.IsAuthenticated)
            {
                var customerID = int.Parse(@User.FindFirst(MySetting.CLAIM_CUSTOMERID)?.Value);
                var user = _context.Users.SingleOrDefault(u => u.Id == customerID);
                var order = _context.OrderDetails.Include(o => o.Order).Where(p => p.OrderStatusId == 1)
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
            return View();
        }

        [Authorize]
        public IActionResult CancelOrder(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var customerID = int.Parse(@User.FindFirst(MySetting.CLAIM_CUSTOMERID)?.Value);
                var user = _context.Users.SingleOrDefault(u => u.Id == customerID);
                var order = _context.OrderDetails.Where(p => p.Id == Id).Where(u => u.Order.UserId == user.Id).FirstOrDefault();
                order.OrderStatusId = 5;
                _context.Update(order);
                _context.SaveChanges();
                return RedirectToAction("Index", "PreparingGoods");
            }
            return View();
        }
    }
}

