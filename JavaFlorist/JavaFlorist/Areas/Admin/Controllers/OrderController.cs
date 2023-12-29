using System;
using JavaFlorist.Models;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class OrderController:Controller
	{
		private readonly DataContext _context;
		public OrderController(DataContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult Index()
		{
            
            var data = _context.OrderDetails.Include(p => p.OrderStatus).Include(p => p.Product).ThenInclude(p => p.Discount).Include(p => p.Order).ThenInclude(p => p.Users).OrderByDescending(p => p.Id);
			return View(data);
		}

		[HttpGet]
		public IActionResult ChangeStatus(int Id)
		{
            ViewBag.OrderStatus = new SelectList(_context.OrderStatuses, "Id", "NameStatus");
            var data = _context.OrderDetails.Where(d => d.Id == Id).FirstOrDefault();
			return View(data);
		}

		[HttpPost]
		public async Task<IActionResult> ChangeStatus(int Id, OrderDetailModel orderDetail)
		{
			if (ModelState.IsValid)
			{
				var data = _context.OrderDetails.Where(p => p.Id == Id).FirstOrDefault();
				data.OrderStatusId = orderDetail.OrderStatusId;
                _context.Update(data);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }

        }

    }
}

