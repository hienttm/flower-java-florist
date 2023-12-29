using System;
using JavaFlorist.Helpers;
using JavaFlorist.Models;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JavaFlorist.Controllers
{
	public class ProductController:Controller
	{
		private readonly DataContext _context;
		public ProductController (DataContext context)
		{
			_context = context;
		}
		public IActionResult Index(int? Id)
		{
			var data = _context.Products.AsQueryable();
			if (Id.HasValue)
			{
				data = data.Where(d => d.OccasionId == Id);
			}
			var result = data.Select(r => new ProductViewModel
			{
				Id = r.Id,
				Name = r.Name,
				Description = r.Description,
				Price = r.Price,
				Discount = decimal.Parse(((r.Price * (100 - r.Discount.Discount)) / 100).ToString("0.00")),
                Slug = r.Slug,
				Image = r.Thumb,
				Occasion = r.Occasion.Name,
			});
			return View(result);
		}
		public IActionResult Detail(string Slug)
		{
			if(Slug != null)
			{
                var data = _context.Products.Include(p => p.Occasion).Include(p => p.Discount).Where(p => p.Status == 1).SingleOrDefault(p => p.Slug == Slug);
				var result = new ProductDetailViewModel
				{
					Id = data.Id,
					Name = data.Name,
					Slug = data.Slug,
					Image = data.Thumb,
					Occasion = data.Occasion.Name,
					Price = data.Price,
                    Discount = decimal.Parse(((data.Price * (100 - data.Discount.Discount)) / 100).ToString("0.00")),
                    Description = data.Description,
                    Content = data.Content,
                };
                return View(result);
            }
			return View();
		}

		public IActionResult Search (string find)
		{
            var data = _context.Products.AsQueryable();

            if (find != null)
            {
                data = data.Where(p => p.Name.Contains(find));
            }
            var result = data.Select(r => new ProductViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Price = r.Price,
                Discount = decimal.Parse(((r.Price * (100 - r.Discount.Discount)) / 100).ToString("0.00")),
                Slug = r.Slug,
                Image = r.Thumb,
                Occasion = r.Occasion.Name,
            });
            return View(result);

        }

        // filter
        [HttpGet]
        public IActionResult FilterProducts(string query)
        {
			var products = _context.Products.Where(p => p.Status == 1);
            switch (query)
            {
                case "Newest":
                    products = products.OrderByDescending(p => p.Id);
                    break;
                case "Old":
                    products = products.OrderBy(p => p.Id);
                    break;
                case "LowestPrice":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "HighestPrice":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    // Mặc định hiển thị tất cả sản phẩm
                    break;
            }
            var result = products.Select(r => new ProductViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Price = r.Price,
                Discount = decimal.Parse(((r.Price * (100 - r.Discount.Discount)) / 100).ToString("0.00")),
                Slug = r.Slug,
                Image = r.Thumb,
                Occasion = r.Occasion.Name,
            });
            return View(result);
        }

        
		[HttpPost]
        [Authorize]
        public IActionResult VoteRate(string comment, int rating, int product)
		{
			var UserId = int.Parse(@User.FindFirst(MySetting.CLAIM_CUSTOMERID)?.Value);
			var rate = _context.Rates.Where(p => p.ProductId == product).Where(p => p.UserId == UserId).FirstOrDefault();
            if (rating == 0)
            {
                return Json(new { success = false, message = "Vui long vote sao" });
            }
            if (rate != null)
			{
                var review = new RateModel
                {
                    UserId = UserId,
                    Comment = comment,
                    Star = rating,
                    ProductId = product,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };


                _context.Add(review);
                _context.SaveChanges();
                return Json(new { success = true, message = "Đánh giá thành công" });
            }
			else
			{
                if (rating == 0)
                {
                    return Json(new { success = false, message = "Vui long vote sao" });
                }
                var review = new RateModel
				{
					UserId = UserId,
					Comment = comment,
					Star = rating,
					ProductId = product,
					CreateTime = DateTime.Now,
					UpdateTime = DateTime.Now
				};
				

				_context.Add(review);
				_context.SaveChanges();
				return Json(new { success = true, message = "Đánh giá thành công" });
			}
        }

        [HttpGet]
        public IActionResult FilterByPrice(decimal Price)
        {
            var data = _context.Products.Include(d => d.Discount).Include(d => d.Occasion).Where(p => p.Price < Price).ToList();
            var result = data.Select(r => new ProductViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Price = r.Price,
                Discount = decimal.Parse(((r.Price * (100 - r.Discount.Discount)) / 100).ToString("0.00")),
                Slug = r.Slug,
                Image = r.Thumb,
                Occasion = r.Occasion.Name,
            });
            return PartialView("ProductItem", result);
        }

        [HttpGet]
        public IActionResult ViewMore()
        {
            var product = _context.Products.Include(p => p.Rates).Where(p => p.Rates.Any(r => r.Star >= 4)).AsQueryable();
            var result = product.Select(r => new ProductViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Image = r.Thumb,
                Slug = r.Slug,
                Price = r.Price,
                Discount = decimal.Parse(((r.Price * (100 - r.Discount.Discount)) / 100).ToString("0.00")),
            });
            return View(result);
        }

    }
}

