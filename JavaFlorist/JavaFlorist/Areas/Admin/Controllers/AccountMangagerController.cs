using System;
using AutoMapper;
using JavaFlorist.Helpers;
using JavaFlorist.Models;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class AccountMangagerController:Controller
	{
		private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AccountMangagerController(DataContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_mapper = mapper;
            _webHostEnvironment = webHostEnvironment;

        }
		public IActionResult Index()
		{
			var data = _context.Users.Where(d => d.Role != "customer").ToList();
			var result = data.Select(r => new UserViewModel
			{
				Id = r.Id,
				Avatar = r.Avatar,
				Username = r.Username,
				Email = r.Email,
				Password = r.Password,
				Roles = r.Role,

			});
			return View(result);
		}

		[HttpGet]
		public IActionResult ChangePassword(UserViewModel user, int Id)
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserModel user)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin người dùng từ cơ sở dữ liệu
                var data = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

                if (data != null)
                {
                    if (user.Password == user.ReapeatPassword)
                    {
                        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                        data.Password = hashedPassword;

                        // Cập nhật thông tin người dùng trong context
                        _context.Update(data);

                        // Lưu các thay đổi vào cơ sở dữ liệu
                        await _context.SaveChangesAsync();

                        // Điều hướng về trang danh sách tài khoản
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ChangePassword = "Change False";
                        return View();
                    }
                }
                else
                {
                    return NotFound(); // Người dùng không tồn tại trong cơ sở dữ liệu
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
                return BadRequest(errorMessage); // Trả về lỗi nếu dữ liệu không hợp lệ
            }
        }

        [HttpGet]
        public IActionResult AddAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAccount(UserViewModel models, IFormFile imageUpload)
        {
            ModelState.Remove("ImageUpload");
            ModelState.Remove("ReapeatPassword");
            if (ModelState.IsValid)
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(models.Password);
                var data = _context.Users.Where(p => p.Email == models.Email).Count();
                if (data > 0)
                {
                    ViewBag.ErrorRegister = "false";
                    return View();
                }
                else
                {
                    var user = _mapper.Map<UserModel>(models);
                    user.Password = hashedPassword;
                    user.Role = models.Roles;
                    user.IsActive = true; // se xu ly khi dung mail
                    if (imageUpload != null)
                    {
                        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "img/customer");
                        string ImageName = Guid.NewGuid().ToString() + "_" + models.ImageUpload.FileName;
                        string filePath = Path.Combine(uploadsDir, ImageName);

                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        await models.ImageUpload.CopyToAsync(fs);
                        fs.Close();
                        user.Avatar = ImageName;
                    }

                    _context.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
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
                return BadRequest(errorMessage); // Trả về lỗi nếu dữ liệu không hợp lệ
            }
            return View();

        }

        public async Task<IActionResult> DeleteAccount(int Id)
        {
            var user = await _context.Users.FindAsync(Id);
            if (!string.Equals(user.Avatar, "noname.jpg"))
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "img/customer/");
                string filePath = Path.Combine(uploadsDir, user.Avatar);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}

