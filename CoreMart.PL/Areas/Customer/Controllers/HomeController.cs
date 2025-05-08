using CoreMart.BLL.Repository.Implementation;
using CoreMart.BLL.Repository.Interface;
using CoreMart.DAL.Context;
using CoreMart.DAL.Migrations;
using CoreMart.DAL.Models;
using CoreMart.DAL.ViewModels;
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

        public HomeController(IUnitOfWork unitOfWork, IProductRepository productRepository, CoreMartDbContext context)
        {
            this.unitOfWork = unitOfWork;
            coreMartDb = context;
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products = await unitOfWork.Products.GetAllAsync();

            //return Json(new { Product = products });
            return View(products);
        }



        public async Task<IActionResult> GetData()
        {
            var products = await unitOfWork.Products.GetAllAsync();


            //int ? completeid = 0;
            //decimal minprice = 0;
            //decimal maxprice = 0;

            //var query = from p in coreMartDb.Products
            //            join c in coreMartDb.Categories on p.CategoryId equals c.Id
            //            join b in coreMartDb.Brands on p.BrandId equals b.Id
            //            where p.Id == completeid && (p.Price >= minprice && p.Price <= maxprice)
            //            select new
            //            {
            //                Id = p.Id,
            //                Name = p.Name,
            //                Description = p.Description,
            //                Price = p.Price,
            //                CategoryName = c.Name,
            //                BrandName = b.Name,
            //                ImageURL = p.ImageURL
            //           };

            return Json(new { products });
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await unitOfWork.Products.GetFirstorDefault(id);
            if (product == null)
                return NotFound();



            return View(product);

        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public async Task<IActionResult> Details(ShoppingCart shoppingcart)
        //{
        //    var claimsIdentity = (ClaimsIdentity)User.Identity;
        //    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        //    shoppingcart.CustomerId = claim.Value;


        //    unitOfWork.ShoppingCart.Add(shoppingcart);
        //    await unitOfWork.CompleteAsync();

        //    return RedirectToAction("Index");







        //    //ShoppingCart CartObj = unitOfWork.ShoppingCart.GetFirstorDefault(
        //    //    u => u.CustomerId == claim.Value && u.ProductId == shopping.ProductId
        //    //);

        //    //if (CartObj == null)
        //    //{
        //    //    unitOfWork.ShoppingCart.Add(shopping);
        //    //}
        //    //else
        //    //{
        //    //    unitOfWork.ShoppingCart.IncreaseCount(CartObj, shopping.Count);
        //    //}


        // }

    }
    }


