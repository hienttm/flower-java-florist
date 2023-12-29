using System;
using JavaFlorist.Models;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JavaFlorist.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class DiscountController:Controller
	{
		private readonly DataContext _context;
		public DiscountController(DataContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var data = _context.Discounts.ToList();
			var result = data.Select(r => new DiscountVM
			{
				Id = r.Id,
				Discount = r.Discount
			});
			return View(result);
		}

		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(DiscountVM disc)
		{
			if (ModelState.IsValid)
			{
				var result = new DiscountModel
				{
					Discount = disc.Discount,
				};
				_context.Add(result);
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

		[HttpGet]
		public IActionResult Fix(int Id)
		{
			var data = _context.Discounts.Where(d => d.Id == Id).FirstOrDefault();
			var result = new DiscountVM
			{
				Id = data.Id,
				Discount = data.Discount,
			};
			return View(result);
		}

		[HttpPost]
		public async Task<IActionResult> Fix(int Id, DiscountVM disc)
		{
			if (ModelState.IsValid)
			{
                var data = _context.Discounts.Where(d => d.Id == Id).FirstOrDefault();
                if (data != null)
                {
                    data.Discount = disc.Discount;
                    _context.Update(data);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
				}
				else
				{
					return NotFound();
				}
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

        public async Task<IActionResult> Delete(int Id)
        {
            var Discount = await _context.Discounts.FindAsync(Id);
            _context.Discounts.Remove(Discount);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

