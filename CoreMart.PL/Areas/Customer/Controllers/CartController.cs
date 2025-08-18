using CoreMart.DAL.Context;
using CoreMart.DAL.Models;
using CoreMart.DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
using Stripe.Checkout;
using CoreMart.BLL.Repository.Interface;

namespace CoreMart.PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CoreMartDbContext coreMartDbContext;
        private const string SHOPPING_CART_SESSION = "ShoppingCartSession";

        public DAL.ViewModels.ShoppingCartVM shoppingCartVM { get; set; }
        public int TotalCarts { get; set; }

        public CartController(IUnitOfWork unitOfWork, CoreMartDbContext coreMartDbContext)
        {
            this.unitOfWork = unitOfWork;
            this.coreMartDbContext = coreMartDbContext;
        }

        private string GetCurrentUserId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            return claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        private async Task UpdateCartSession()
        {
            var userId = GetCurrentUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                var cartCount = unitOfWork.ShoppingCart.GetAll(x => x.CustomerId == userId).Sum(c => c.Count);
                HttpContext.Session.SetInt32(SHOPPING_CART_SESSION, cartCount);
            }
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            ShoppingCartVM vvv = new ShoppingCartVM()
            {
                CartList = unitOfWork.ShoppingCart.GetAll(u => u.CustomerId == userId, Includeword: "Product")
            };

            foreach (var item in vvv.CartList)
            {
                vvv.TotalCarts += (item.Count * item.Product.Price);
            }

            // Update session with current cart count
            await UpdateCartSession();

            return View(vvv);
        }

        public async Task<IActionResult> Plus(int cartid)
        {
            var shoppingcart = unitOfWork.ShoppingCart.GetById(c => c.Id == cartid);
            if (shoppingcart == null)
            {
                return NotFound();
            }

            unitOfWork.ShoppingCart.IncreaseCount(shoppingcart, 1);
            await unitOfWork.CompleteAsync();

            // Update session after database change
            await UpdateCartSession();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Minus(int cartid)
        {
            var shoppingcart = unitOfWork.ShoppingCart.GetById(c => c.Id == cartid);
            if (shoppingcart == null)
            {
                return NotFound();
            }

            if (shoppingcart.Count <= 1)
            {
                unitOfWork.ShoppingCart.Remove(shoppingcart);
            }
            else
            {
                unitOfWork.ShoppingCart.DecreaseCount(shoppingcart, 1);
            }

            await unitOfWork.CompleteAsync();

            // Update session after database change
            await UpdateCartSession();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int cartid)
        {
            var shoppingcart = unitOfWork.ShoppingCart.GetById(c => c.Id == cartid);
            if (shoppingcart == null)
            {
                return NotFound();
            }

            unitOfWork.ShoppingCart.Remove(shoppingcart);
            await unitOfWork.CompleteAsync();

            // Update session after database change
            await UpdateCartSession();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Summary()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM()
            {
                CartList = unitOfWork.ShoppingCart.GetAll(u => u.CustomerId == userId, Includeword: "Product"),
                OrderHeader = new()
            };

            shoppingCartVM.OrderHeader.ApplicationUser = unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == userId);

            if (shoppingCartVM.OrderHeader.ApplicationUser != null)
            {
                shoppingCartVM.OrderHeader.FullName = shoppingCartVM.OrderHeader.ApplicationUser.FullName;
                shoppingCartVM.OrderHeader.Address = shoppingCartVM.OrderHeader.ApplicationUser.Address;
                shoppingCartVM.OrderHeader.Phone = shoppingCartVM.OrderHeader.ApplicationUser.Phone;
            }

            foreach (var item in shoppingCartVM.CartList)
            {
                shoppingCartVM.OrderHeader.TotalAmount += (item.Count * item.Product.Price);
            }

            return View(shoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> POSTSummary(ShoppingCartVM shoppingCartVM)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            shoppingCartVM.CartList = unitOfWork.ShoppingCart.GetAll(u => u.CustomerId == userId, Includeword: "Product");

            shoppingCartVM.OrderHeader.OrderStatus = "Pending";
            shoppingCartVM.OrderHeader.paymentStatus = "Pending";
            shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            shoppingCartVM.OrderHeader.CustomerId = userId;

            // Reset TotalAmount to ensure accurate calculation
            shoppingCartVM.OrderHeader.TotalAmount = 0;
            foreach (var item in shoppingCartVM.CartList)
            {
                shoppingCartVM.OrderHeader.TotalAmount += (item.Count * item.Product.Price);
            }

            await unitOfWork.OrderHeader.AddAsync(shoppingCartVM.OrderHeader);
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
            }
            await unitOfWork.CompleteAsync();

            var domain = "https://localhost:7203/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
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
            if (orderHeader == null)
            {
                return NotFound();
            }

            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                orderHeader.PaymentIntendId = session.PaymentIntentId;
                unitOfWork.OrderHeader.UpdateOrderStatus(id, "Approved", "Approved");
                await unitOfWork.CompleteAsync();
            }

            // Clear cart after successful order
            List<ShoppingCart> shoppingCarts = unitOfWork.ShoppingCart.GetAll(u => u.CustomerId == orderHeader.CustomerId).ToList();
            unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            await unitOfWork.CompleteAsync();

            // Clear session cart count
            HttpContext.Session.SetInt32(SHOPPING_CART_SESSION, 0);

            return View(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (quantity <= 0)
            {
                TempData["Error"] = "Invalid quantity!";
                return RedirectToAction("Index", "Home");
            }

            var product = await unitOfWork.Products.GetFirstorDefault(productId);
            if (product == null)
            {
                TempData["Error"] = "Product not found!";
                return RedirectToAction("Index", "Home");
            }

            var existingCart = unitOfWork.ShoppingCart.GetById(
                c => c.ProductId == productId && c.CustomerId == userId
            );

            if (existingCart != null)
            {
                existingCart.Count += quantity;
            }
            else
            {
                ShoppingCart newCartItem = new DAL.Models.ShoppingCart
                {
                    CustomerId = userId,
                    ProductId = productId,
                    Count = quantity,
                    PriceAtThatTime = product.Price
                };
                unitOfWork.ShoppingCart.Add(newCartItem);
            }

            await unitOfWork.CompleteAsync();

            // Update session after database change
            await UpdateCartSession();

            TempData["Message"] = "Product added to cart!";
            return RedirectToAction("Index", "Home");
        }

        // Helper method to get cart count for session (can be called from other controllers)
        public int GetCartCount()
        {
            var sessionCount = HttpContext.Session.GetInt32(SHOPPING_CART_SESSION);
            if (sessionCount.HasValue)
            {
                return sessionCount.Value;
            }

            // If session is empty, get from database
            var userId = GetCurrentUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                var count = unitOfWork.ShoppingCart.GetAll(x => x.CustomerId == userId).Sum(c => c.Count);
                HttpContext.Session.SetInt32(SHOPPING_CART_SESSION, count);
                return count;
            }

            return 0;
        }
    }
}