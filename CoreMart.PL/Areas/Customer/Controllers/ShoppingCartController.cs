using CoreMart.BLL.Repository.Implementation;
using CoreMart.BLL.Repository.Interface;
using CoreMart.DAL.Context;
using CoreMart.DAL.Models;
using CoreMart.DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreMart.PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]

    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CoreMartDbContext coreMartDbContext;
        public ShoppingCartVM shoppingCartVM { get; set; }
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



            var CartList = unitOfWork.ShoppingCart.GetAll(u => u.CustomerId == claim.Value, Includeword: "Product");

            decimal totalAmount = CartList.Sum(item => item.PriceAtThatTime * item.Count);

            ViewData["TotalAmount"] = totalAmount;  // تخزين الإجمالي في ViewData

            return View(CartList);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var product=await unitOfWork.Products.GetById(productId);

            var existingCart = unitOfWork.ShoppingCart.GetFirstorDefault(
                    c => c.ProductId == productId && c.CustomerId == claim.Value
                );

            if (existingCart != null)
            {
                existingCart.Count += quantity;
            }
            else
            {
                var newCartItem = new ShoppingCart
                {
                    CustomerId = claim.Value,
                    ProductId = productId,
                    Count = quantity,
                    PriceAtThatTime = product.Price // ✅ السعر بيتسجل هنا
                };
                unitOfWork.ShoppingCart.Add(newCartItem);
            }

            await unitOfWork.CompleteAsync();

            TempData["Message"] = "Product added to cart!";
            return RedirectToAction("Index", "Home");
        }


    }
}
