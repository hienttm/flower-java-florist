using System;
using JavaFlorist.Helpers;
using System.Security.Claims;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JavaFlorist.Areas.Manager.Controllers
{
	[Area("Manager")]
    [Authorize]
	public class LoginController:Controller
	{
		private readonly DataContext _context;
		public LoginController(DataContext context)
		{
			_context = context;
		}
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
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

                if (user.Role == "manager")
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
    }
}

