using System;
using JavaFlorist.Helpers;
using AutoMapper;
using JavaFlorist.Models;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JavaFlorist.Helpers;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Controllers
{
	public class Profile:Controller
	{
		private readonly DataContext _context;
        private readonly IMapper _mapper;
        public Profile(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

        [Authorize]
        public IActionResult Index()
		{
            
			if (User.Identity.IsAuthenticated)
			{
				var customerID = int.Parse(@User.FindFirst(MySetting.CLAIM_CUSTOMERID)?.Value);
				var user = _context.Users.FirstOrDefault(u => u.Id == customerID);
				var getUser = new UserViewModel
				{
					Id = user.Id,
					Username = user.Username,
					Email = user.Email,
					Phone = user.Phone,
					Gender = user.Gender,
					Avatar = user.Avatar,
				};
				return View(getUser);
            }
            return View();
		}

		[Authorize]
		[HttpPost]
		public IActionResult Index(int Id, UserViewModel model, IFormFile Avatar)
		{
            ModelState.Remove("img");
            ModelState.Remove("Avatar");
            ModelState.Remove("Password");
            ModelState.Remove("Gender");
            ModelState.Remove("Email");
            ModelState.Remove("Phone");
            ModelState.Remove("Address");
            ModelState.Remove("ReapeatPassword");

            if (ModelState.IsValid)
            {
                var user = _context.Users.SingleOrDefault(u => u.Id == Id);
                if (user == null)
                {
                    return NotFound(); // Trả về mã lỗi 404 nếu không tìm thấy người dùng
                }

                user.Username = model.Username;
                user.Email = model.Email;
                user.Phone = model.Phone;
                user.Gender = model.Gender;

                if (Avatar != null)
                {
                    user.Avatar = MyUtil.UploadHinh(Avatar, "customer");
                }

                _context.Update(user);
                _context.SaveChanges();

                return RedirectToAction("Index", "Profile");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                var errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult Sidebar()
        {

            if (User.Identity.IsAuthenticated)
            {
                var customerID = int.Parse(@User.FindFirst(MySetting.CLAIM_CUSTOMERID)?.Value);
                var user = _context.Users.SingleOrDefault(u => u.Id == customerID);
                var getUser = new UserViewModel
                {
                    Username = user.Username,
                    Avatar = user.Avatar,
                };
                return View(getUser);
            }
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult CustomerOrder()
        {
            if (User.Identity.IsAuthenticated)
            {
                var customerID = int.Parse(@User.FindFirst(MySetting.CLAIM_CUSTOMERID)?.Value);
                var user = _context.Users.SingleOrDefault(u => u.Id == customerID);
                var order = _context.OrderDetails.Include(o => o.Order).Where(p => p.OrderStatusId == 4)
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

