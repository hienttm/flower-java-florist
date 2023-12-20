using System;
using JavaFlorist.Repository;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Mvc;
using JavaFlorist.Helpers;
using Microsoft.AspNetCore.Authorization;
using JavaFlorist.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using JavaFlorist.Services;

namespace JavaFlorist.Controllers
{
	public class CartController:Controller
	{
		private readonly DataContext _context;
		private readonly PaypalClient _paypalClient;
		private readonly IVnPayService _vnpayService;
		public CartController(DataContext context, PaypalClient paypalClient, IVnPayService vnpayService)
		{
			_context = context;
			_paypalClient = paypalClient;
			_vnpayService = vnpayService;
		}
		
		public List<CartViewModel> Cart => HttpContext.Session.Get<List<CartViewModel>>(MySetting.Cart_Key) ?? new List<CartViewModel>();
		public IActionResult Index()
		{
			return View(Cart);
		}
		public IActionResult AddToCard(int Id, int quantity = 1)
		{
			var cart = Cart;
			var item = cart.SingleOrDefault(p => p.Id == Id);
			if(item == null)
			{
				var product = _context.Products.Include(p => p.Discount).Where(p=> p.Status == 1).SingleOrDefault(p => p.Id == Id);
				if(product == null)
				{
					return NotFound();
				}
				item = new CartViewModel
				{
					Id = product.Id,
					Name = product.Name,
					Price = product.Price,
					Discount = decimal.Parse(((product.Price * (100 - product.Discount.Discount)) / 100).ToString("0.00")),
                    Img = product.Thumb,
					Quantity = quantity,
				};
				cart.Add(item);
            }
			else
			{
				item.Quantity += quantity;
			}
			HttpContext.Session.Set(MySetting.Cart_Key,cart);
			return RedirectToAction("Index");
		}

        public IActionResult MinusCard(int Id, int quantity = 1)
        {
            var cart = Cart;
            var item = cart.SingleOrDefault(p => p.Id == Id);
            if (item != null)
            {
                item.Quantity -= quantity;
				if(item.Quantity <= 0)
				{
                    cart.Remove(item);
                    HttpContext.Session.Set(MySetting.Cart_Key, cart);
                }
            }
            HttpContext.Session.Set(MySetting.Cart_Key, cart);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveCart(int Id)
		{
			var cart = Cart;
            var item = cart.SingleOrDefault(p => p.Id == Id);
			if(item != null)
			{
				cart.Remove(item);
				HttpContext.Session.Set(MySetting.Cart_Key, cart);

            }
            return RedirectToAction("Index");
        }

		[Authorize]
		[HttpGet]
		public IActionResult Checkout()
		{
			if(Cart.Count == 0)
			{
				return Redirect("/");
			}
			ViewBag.PaypalClientId = _paypalClient.ClientId;
			return View(Cart);
		}

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(CheckoutVm model, string payment = "COD")
        {
            ModelState.Remove("img");
            ModelState.Remove("Avatar");
            if (ModelState.IsValid)
			{
				if(payment == "Pay VNPay")
				{
					var vnPayModel = new VnPaymentRequestModel
					{
						Amount = Convert.ToDouble(Cart.Sum(p => p.Total*24000)),
						CreatedDate = DateTime.Now,
						Description = $"{model.UserName} {model.Phone}",
						FullName = model.UserName,
						OrderId = new Random().Next(1000,10000)
					};
					return Redirect(_vnpayService.CreatePaymentUrl(HttpContext, vnPayModel));
				}
				var customerID =int.Parse(@User.FindFirst(MySetting.CLAIM_CUSTOMERID)?.Value);
				var user = new UserModel();
				if (model.LikeCustomer)
				{
					user = _context.Users.SingleOrDefault(u => u.Id == customerID);

				}
				var order = new OrderModel
				{
					UserId = customerID,
					UserName = model.UserName ?? user.Username,
					Address = model.Address ?? user.Address,
					Phone = model.Phone ?? user.Phone,
					DateOrder = DateTime.Now,
					HowToPay = "COD",
					ShipingWay = "Ship",
					Status = 1,
					Note = model.Note
				};
                _context.Database.BeginTransaction();
                try
				{
                    _context.Database.CommitTransaction();
					_context.Add(order);
					_context.SaveChanges();
					var orderdetail = new List<OrderDetailModel>();
					foreach(var item in Cart)
					{
						orderdetail.Add(new OrderDetailModel
						{
							OrderId = order.Id,
							Quantity = item.Quantity,
							Price = item.Price,
							ProductId = item.Id,
							Discount = item.Discount,
                        });
					}
					_context.AddRangeAsync(orderdetail);
					_context.SaveChanges();
					HttpContext.Session.Set<List<CartViewModel>>(MySetting.Cart_Key, new List<CartViewModel>());
					return View("Success");
                }
				catch
				{
					_context.Database.RollbackTransaction();
				}
				
			}
            return View(Cart);
        }

		[Authorize]
		[HttpGet]
		public IActionResult Success()
		{
			return View();
		}

		[Authorize]
		[HttpPost("/Cart/create-paypal-order")]
		public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
		{
			// info order send to paypal
			var Total = Cart.Sum(p => p.Total).ToString();
			var CurrencyUnit = "USD";
			var CodeOrder = "DH" + DateTime.Now.Ticks.ToString();
			try
			{
				var response = await _paypalClient.CreateOrder(Total, CurrencyUnit, CodeOrder);
				return Ok(response);
			}catch(Exception ex)
			{
				var error = new { ex.GetBaseException().Message };
				return BadRequest(error);
			}

        }

		[Authorize]
		[HttpPost("/Cart/capture-paypal-order")]
		public async Task<IActionResult>CapturePaypalOrder(string orderId, CancellationToken cancellationToken, CheckoutVm model)
		{
			try
			{
				var response = await _paypalClient.CaptureOrder(orderId);

                // save database

                ModelState.Remove("img");
                ModelState.Remove("Avatar");
                if (ModelState.IsValid)
                {
                    var customerID = int.Parse(@User.FindFirst(MySetting.CLAIM_CUSTOMERID)?.Value);
                    var user = new UserModel();
                    if (model.LikeCustomer)
                    {
                        user = _context.Users.SingleOrDefault(u => u.Id == customerID);

                    }
                    var order = new OrderModel
                    {
                        UserId = customerID,
                        UserName = model.UserName ?? user.Username,
                        Address = model.Address ?? user.Address,
                        Phone = model.Phone ?? user.Phone,
                        DateOrder = DateTime.Now,
                        HowToPay = "COD",
                        ShipingWay = "Ship",
                        Status = 1,
                        Note = model.Note
                    };
                    _context.Database.BeginTransaction();
                    try
                    {
                        _context.Database.CommitTransaction();
                        _context.Add(order);
                        _context.SaveChanges();
                        var orderdetail = new List<OrderDetailModel>();
                        foreach (var item in Cart)
                        {
                            orderdetail.Add(new OrderDetailModel
                            {
                                OrderId = order.Id,
                                Quantity = item.Quantity,
                                Price = item.Price,
                                ProductId = item.Id,
                                Discount = item.Discount,
                            });
                        }
                        _context.AddRange(orderdetail);
                        _context.SaveChanges();
                        HttpContext.Session.Set<List<CartViewModel>>(MySetting.Cart_Key, new List<CartViewModel>());
                        return View("Success");
                    }
                    catch
                    {
                        _context.Database.RollbackTransaction();
                    }
                    return View(Cart);
                }

                return Ok(response);
			}catch(Exception ex)
			{
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
		}

		[Authorize]
		public IActionResult PaymentFail()
		{
			return View();
		}

		[Authorize]
		public IActionResult PaymentCallBack()
		{
			var response = _vnpayService.PaymentExecute(Request.Query);
			if(response == null || response.VnPayResponseCode != "00") {
				return RedirectToAction("PaymentFail");
			}

			// save order database
			return RedirectToAction("Success");
		}

    }
}

