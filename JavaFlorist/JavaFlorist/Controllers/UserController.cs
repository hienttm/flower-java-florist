using System;
using System.Security.Claims;
using AutoMapper;
using JavaFlorist.Helpers;
using JavaFlorist.Models;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserViewModel models, IFormFile Avatar)
        {
            ModelState.Remove("img");
            ModelState.Remove("Avatar");

            if (ModelState.IsValid)
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(models.Password);

                var user = _mapper.Map<UserModel>(models);
                user.Password = hashedPassword;
                user.IsActive = true; // se xu ly khi dung mail
                user.Role = "user";
                user.Avatar = "avatar-2.png";
                if (Avatar != null)
                {
                    user.Avatar = MyUtil.UploadHinh(Avatar, "customer");
                }
                _context.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login", "User");
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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _context.Users.FirstOrDefault(a => a.Email == model.Email);
            if (user == null)
            {
                return Ok("Email không tồn tại");
            }

            // Sử dụng BCrypt để kiểm tra mật khẩu
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return Ok("Mật khẩu không đúng");
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("Avatar", user.Avatar),
            new Claim(ClaimTypes.MobilePhone, user.Phone),
            new Claim(ClaimTypes.StreetAddress, user.Address),
            new Claim(ClaimTypes.Country, user.City),
            new Claim("FirstName", user.Firstname),
            new Claim("LastName", user.Lastname),
            new Claim(MySetting.CLAIM_CUSTOMERID, user.Id.ToString()),
        };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }
}

