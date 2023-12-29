    using System;
using AutoMapper;
using JavaFlorist.Helpers;
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
    public class ProductController : Controller
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(DataContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var data = _context.Products.Include(p => p.Discount).Include(p => p.Occasion).ToList();
            return View(data);
        }
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Occasions = new SelectList(_context.Occasions, "Id", "Name");
            ViewBag.Discounts = new SelectList(_context.Discounts, "Id", "Discount");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductVM productVM, IFormFile imageUpload)
        {
            ModelState.Remove("ImageUpload");
           
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<ProductModel>(productVM);
                if (imageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "img/");
                    string ImageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, ImageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Thumb = ImageName;
                }
                _context.Add(product);
                _context.SaveChanges();
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
        public IActionResult Fix(int Id, ProductModel product)
        {
            ViewBag.Occasions = new SelectList(_context.Occasions, "Id", "Name");
            ViewBag.Discounts = new SelectList(_context.Discounts, "Id", "Discount");
            var data = _context.Products.Include(p => p.Occasion).Include(p => p.Discount).Where(p => p.Id == Id).FirstOrDefault();
           
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Fix (int Id, ProductModel product, IFormFile imageUpload)
        {
            ModelState.Remove("ImageUpload");
            if (ModelState.IsValid)
            {
                if (imageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "img/ProductImgAdmin/");
                    string ImageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, ImageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Thumb = ImageName;
                }
                _context.Update(product);
                _context.SaveChanges();
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
            return View();
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            if (!string.Equals(product.Thumb, "noname.jpg"))
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "img/ProductImgAdmin/");
                string filePath = Path.Combine(uploadsDir, product.Thumb);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

