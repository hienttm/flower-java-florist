using System;
using JavaFlorist.Helpers;
using System.Security.Claims;
using JavaFlorist.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace JavaFlorist.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize]
	public class LoginController:Controller
	{
		private readonly DataContext _context;
		public LoginController(DataContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
            var user = _context.Users.Where(a => a.Email == model.Email).FirstOrDefault();
            if (user == null)
            {
                ViewBag.LoginAdminFalse = "Login Falses";
                return RedirectToAction("Index");
            }

            // Sử dụng BCrypt để kiểm tra mật khẩu
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                ViewBag.LoginAdminFalse = "Login Falses";
                return RedirectToAction("Index");
            }

            if(user.Role == "admin")
			{
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("Avatar", user.Avatar),
                new Claim(ClaimTypes.MobilePhone, user.Phone),
                new Claim(ClaimTypes.StreetAddress, user.Address),
                new Claim(MySetting.CLAIM_CUSTOMERID, user.Id.ToString()),
            };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                ViewBag.LoginAdminSuccess = "Login Success";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.LoginAdminFalse = "Login Falses";
                return RedirectToAction("Index");
            }
        }

	}
}

