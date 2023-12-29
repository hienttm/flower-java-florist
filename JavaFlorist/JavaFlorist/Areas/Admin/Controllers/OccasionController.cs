using System;
using JavaFlorist.Models;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class OccasionController:Controller
	{
		private readonly DataContext _context;
		public OccasionController (DataContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
            var data = _context.Occasions.ToList();
            var result = data.Select(r => new OccasionVM
            {
				Id = r.Id,
                Name = r.Name,
                Slug = r.Slug,
                Status = r.Status,
            });
            return View(result);
        }

		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(OccasionVM occasion)
		{
			if (ModelState.IsValid)
			{
				var result = new OccasionModel
				{
					Name = occasion.Name,
					Slug = occasion.Slug,
					Status = occasion.Status,
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
		public IActionResult Fix (int Id)
		{
			var data = _context.Occasions.Where(d => d.Id == Id).FirstOrDefault();
			var result = new OccasionVM
			{
				Id = data.Id,
				Name = data.Name,
				Slug = data.Slug,
				Status = data.Status,
			};
			return View(result);
		}

		[HttpPost]
		public async Task<IActionResult> Fix (int Id, OccasionVM occasion)
		{
			if (ModelState.IsValid)
			{
				var data = _context.Occasions.Where(d => d.Id == Id).FirstOrDefault();
				if(data != null)
				{
					data.Name = occasion.Name;
					data.Slug = occasion.Slug;
					data.Status = occasion.Status;
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
            var occasion = await _context.Occasions.FindAsync(Id);
            _context.Occasions.Remove(occasion);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}

