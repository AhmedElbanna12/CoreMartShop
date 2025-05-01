using CoreMart.BLL.Repository.Implementation;
using CoreMart.BLL.Repository.Interface;
using CoreMart.DAL.Context;
using CoreMart.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreMart.PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IProductRepository productRepository;
        private readonly CoreMartDbContext coreMartDb;

        public HomeController(IUnitOfWork unitOfWork ,CoreMartDbContext context,IProductRepository productRepository)
        {
            this.unitOfWork = unitOfWork;
            coreMartDb = context;
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products =await unitOfWork.Products.GetAllAsync();
  
            return View(products);
        }

        public async Task<IActionResult> GetData()
        {
            var products = await unitOfWork.Products.GetAllAsync();

            return Json(new { products });
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await unitOfWork.Products.GetById(id);
            if (product == null)
                return NotFound();

            

            return View(product);

        }




        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public async Task<IActionResult> Details(ShoppingCart shopping)
        //{
        //    var claimsIdentity = (ClaimsIdentity)User.Identity;
        //    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        //    shopping.CustomerId = claim.Value;


        //    ShoppingCart CartObj =  unitOfWork.ShoppingCart.GetFirstorDefault(
        //        u => u.CustomerId == claim.Value && u.ProductId == shopping.ProductId
        //    );

        //    if (CartObj == null)
        //    {
        //         unitOfWork.ShoppingCart.Add(shopping);
        //    }
        //    else
        //    {
        //        unitOfWork.ShoppingCart.IncreaseCount(CartObj, shopping.Count);
        //    }

        //    //_unitofwork.ShoppingCart.Add(shopping);
        //    await unitOfWork.CompleteAsync();

        //    return RedirectToAction("Index");
        //}


    }
}
