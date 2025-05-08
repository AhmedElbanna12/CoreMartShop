using CoreMart.BLL.Repository.Interface;
using CoreMart.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CoreMart.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await unitOfWork.Products.GetAllAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await unitOfWork.Categories.GetCategories(), "Id", "Name");
            ViewBag.Brands = new SelectList(await unitOfWork.Brands.GetBrands(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadPath = Path.Combine(RootPath, @"Images\Products");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    var ext = Path.GetExtension(file.FileName);

                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName + ext), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    product.ImageURL = @"Images\Products\" + fileName + ext;
                }

                unitOfWork.Products.Add(product);
              await unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(await unitOfWork.Categories.GetCategories(), "Id", "Name");
            ViewBag.Brands = new SelectList(await unitOfWork.Brands.GetBrands(), "Id", "Name");
            return View("Create", product);
        }


        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            var productInDB = await unitOfWork.Products.GetFirstorDefault(id);
            if(productInDB != null)
            {
                ViewBag.Categories = new SelectList(await unitOfWork.Categories.GetCategories(), "Id", "Name");
                ViewBag.Brands = new SelectList(await unitOfWork.Brands.GetBrands(), "Id", "Name");

                return View(productInDB);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product ,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadPath = Path.Combine(RootPath, @"Images\Products");
                    var ext = Path.GetExtension(file.FileName);
                    var newImagePath = Path.Combine(uploadPath, fileName + ext);

                    // حذف الصورة القديمة إذا كانت موجودة
                    if (product.ImageURL != null)
                    {
                        var oldImagePath = Path.Combine(RootPath, product.ImageURL.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(newImagePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }



                    product.ImageURL = @"Images\Products\" + fileName + ext;

                }

                await unitOfWork.Products.UpdateAsync(product);
                await unitOfWork.CompleteAsync();
                return RedirectToAction("Index");

            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var Product =await  unitOfWork.Products.GetFirstorDefault(id);
            if (Product == null)
            {
                return NotFound();
            }

            await unitOfWork.Products.DeleteAsync(Product);
            await unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }






    }
}
