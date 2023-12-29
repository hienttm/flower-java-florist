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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(DataContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserViewModel models, IFormFile imageUpload)
        {
            ModelState.Remove("ImageUpload");
            ModelState.Remove("Avatar");
            ModelState.Remove("ReapeatPassword");
            if (ModelState.IsValid)
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(models.Password);
                var data = _context.Users.Where(p => p.Email == models.Email).Count();
                if(data > 0)
                {
                    ViewBag.ErrorRegister = "false";
                    return View();
                }
                else
                {
                    var user = _mapper.Map<UserModel>(models);
                    user.Password = hashedPassword;
                    user.IsActive = true; // se xu ly khi dung mail
                    user.Role = "customer";
                    user.Avatar = "avatar-2.png";
                    if (imageUpload != null)
                    {
                        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "img/customer");
                        string ImageName = Guid.NewGuid().ToString() + "_" + models.ImageUpload.FileName;
                        string filePath = Path.Combine(uploadsDir, ImageName);

                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        models.ImageUpload.CopyToAsync(fs);
                        fs.Close();
                        user.Avatar = ImageName;
                    }
                    _context.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction("Login", "User");
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
                ViewBag.LoginFalse = "Login Falses";
                return View();
            }

            // Sử dụng BCrypt để kiểm tra mật khẩu
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                ViewBag.LoginFalse = "Login Falses";
                return View();
            }

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
            ViewBag.LoginSuccess = "Login Success";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Set<List<CartViewModel>>(MySetting.Cart_Key, new List<CartViewModel>());
            return RedirectToAction("Index", "Home");
        }

        public IActionResult BecomePartner()
        {
            return View();
        }
    }
}

