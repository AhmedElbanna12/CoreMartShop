using CoreMart.BLL.Repository.Implementation;
using CoreMart.BLL.Repository.Interface;
using CoreMart.DAL.Context;
using CoreMart.DAL.Models;
using CoreMart.DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using Stripe.Checkout;


namespace CoreMart.PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]

    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CoreMartDbContext coreMartDbContext;
        public DAL.ViewModels.ShoppingCartVM shoppingCartVM { get; set; }
        public int TotalCarts { get; set; }

        public CartController(IUnitOfWork unitOfWork , CoreMartDbContext coreMartDbContext)
        {
            this.unitOfWork = unitOfWork;
            this.coreMartDbContext = coreMartDbContext;
        }


        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            ShoppingCartVM  vvv= new ShoppingCartVM()
            {
               CartList = unitOfWork.ShoppingCart.GetAll(u => u.CustomerId == claim.Value, Includeword: "Product")
            };


            foreach (var item in vvv.CartList)
            {
                vvv.TotalCarts += (item.Count * item.Product.Price);

            }
            return View(vvv);
          

            //decimal totalAmount = CartList.Sum(item => item.PriceAtThatTime * item.Count);

            //ViewData["TotalAmount"] = totalAmount; 

            //return View(CartList);
        }


        public  async Task<IActionResult> Plus (int cartid)
        {
            var shoppingcart = unitOfWork.ShoppingCart.GetById(  c => c.Id == cartid);
            unitOfWork.ShoppingCart.IncreaseCount(shoppingcart,1);
            await unitOfWork.CompleteAsync(); 
            return RedirectToAction("Index");
        }

        public async Task< IActionResult> Minus(int cartid)
        {
            var shoppingcart = unitOfWork.ShoppingCart.GetById(c => c.Id == cartid);

            if (shoppingcart.Count <=1)
            {
                unitOfWork.ShoppingCart.Remove(shoppingcart);
                var count = unitOfWork.ShoppingCart.GetAll(x => x.CustomerId == shoppingcart.CustomerId).ToList().Count()-1;
                HttpContext.Session.SetInt32("ShoppingCartSession", count);



              //await unitOfWork.CompleteAsync();
              //  return RedirectToAction("Index","Home");
            }
            else
            {
                unitOfWork.ShoppingCart.DecreaseCount(shoppingcart, 1);

            }
           await  unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int cartid)
        {
            var shoppingcart = unitOfWork.ShoppingCart.GetById(c => c.Id == cartid);
            unitOfWork.ShoppingCart.Remove(shoppingcart);
            await unitOfWork.CompleteAsync();
            var count = unitOfWork.ShoppingCart.GetAll(x => x.CustomerId == shoppingcart.CustomerId).ToList().Count();
            HttpContext.Session.SetInt32("ShoppingCartSession", count);
            return RedirectToAction("Index");
        }




        [HttpGet]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM()
            {
                CartList = unitOfWork.ShoppingCart.GetAll(u => u.CustomerId == claim.Value, Includeword: "Product"),
                OrderHeader = new()
            };

            shoppingCartVM.OrderHeader.ApplicationUser = unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);

            shoppingCartVM.OrderHeader.FullName = shoppingCartVM.OrderHeader.ApplicationUser.FullName;
            shoppingCartVM.OrderHeader.Address = shoppingCartVM.OrderHeader.ApplicationUser.Address;
            shoppingCartVM.OrderHeader.ApplicationUser.Email = shoppingCartVM.OrderHeader.ApplicationUser.Email;
            shoppingCartVM.OrderHeader.Phone = shoppingCartVM.OrderHeader.ApplicationUser.Phone;

            foreach (var item in shoppingCartVM.CartList)
            {
                shoppingCartVM.OrderHeader.TotalAmount += (item.Count * item.Product.Price);
            }

            return View(shoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> POSTSummary(ShoppingCartVM shoppingCartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCartVM.CartList = unitOfWork.ShoppingCart.GetAll(u => u.CustomerId == claim.Value, Includeword: "Product");

            shoppingCartVM.OrderHeader.OrderStatus = "Pending";
            shoppingCartVM.OrderHeader.paymentStatus = "Pending";
            shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            shoppingCartVM.OrderHeader.CustomerId = claim.Value;

            foreach (var item in shoppingCartVM.CartList)
            {
                shoppingCartVM.OrderHeader.TotalAmount += (item.Count * item.Product.Price);
            }

            await  unitOfWork.OrderHeader.AddAsync(shoppingCartVM.OrderHeader);
            await unitOfWork.CompleteAsync();

            foreach (var item in shoppingCartVM.CartList)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = item.ProductId,
                    OrderId = shoppingCartVM.OrderHeader.Id,
                    Price = item.Product.Price,
                    Count = item.Count
                };
                await unitOfWork.OrderDetails.AddAsync(orderDetails);
                await unitOfWork.CompleteAsync();

            }


            var domain = "https://localhost:7203/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(), // Add items here
                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={shoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index",
            };
            foreach (var item in shoppingCartVM.CartList)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);
            shoppingCartVM.OrderHeader.SessionId = session.Id;
            shoppingCartVM.OrderHeader.PaymentIntendId = session.PaymentIntentId;

            await unitOfWork.CompleteAsync();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            OrderHeader orderHeader = await unitOfWork.OrderHeader.GetByIdAsync(id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                orderHeader.PaymentIntendId = session.PaymentIntentId;
                unitOfWork.OrderHeader.UpdateOrderStatus(id, "Approved", "Approved");
              //  orderHeader.PaymentIntendId = session.PaymentIntentId;
               await unitOfWork.CompleteAsync();
            }


             

            List<ShoppingCart> shoppingCarts = unitOfWork.ShoppingCart.GetAll(u => u.CustomerId == orderHeader.CustomerId).ToList();
            unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
           await unitOfWork.CompleteAsync();
            return View(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var product = await unitOfWork.Products.GetFirstorDefault(productId);

            var existingCart = unitOfWork.ShoppingCart.GetById(
                    c => c.ProductId == productId && c.CustomerId == claim.Value
                );

            if (existingCart != null)
            {
                existingCart.Count += quantity;
            }
            else
            {
                ShoppingCart newCartItem = new DAL.Models.ShoppingCart
                {
                    CustomerId = claim.Value,
                    ProductId = productId,
                    Count = quantity,
                    PriceAtThatTime = product.Price
                };
                unitOfWork.ShoppingCart.Add(newCartItem);
            }

            HttpContext.Session.SetInt32("ShoppingCartSession",
                unitOfWork.ShoppingCart.GetAll(x => x.CustomerId == claim.Value).ToList().Count
            );

            await unitOfWork.CompleteAsync();

            TempData["Message"] = "Product added to cart!";
            return RedirectToAction("Index", "Home");
        }


    }
}
