using CoreMart.BLL.Repository.Interface;
using CoreMart.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreMart.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public BrandController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var brands = await unitOfWork.Brands.GetAllAsync();
            return View(brands);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadPath = Path.Combine(RootPath, @"Images\Brands");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    var ext = Path.GetExtension(file.FileName);

                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName + ext), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    brand.ImageURL = @"Images\Brands\" + fileName + ext;
                }

                await unitOfWork.Brands.AddAsync(brand);
                await unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }


            return View("Create", brand);
        }


        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            var brandInDB = await unitOfWork.Brands.GetByIdAsync(id);
            if (brandInDB != null)
            {
                return View(brandInDB);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Brand brand, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadPath = Path.Combine(RootPath, @"Images\Brands");
                    var ext = Path.GetExtension(file.FileName);
                    var newImagePath = Path.Combine(uploadPath, fileName + ext);

                    // حذف الصورة القديمة إذا كانت موجودة
                    if (brand.ImageURL != null)
                    {
                        var oldImagePath = Path.Combine(RootPath, brand.ImageURL.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(newImagePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }



                    brand.ImageURL = @"Images\Brands\" + fileName + ext;

                }

                await unitOfWork.Brands.UpdateAsync(brand);
                await unitOfWork.CompleteAsync();
                return RedirectToAction("Index");

            }
            return View(brand);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var Brand = await unitOfWork.Brands.GetByIdAsync(id);
            if (Brand != null)
            {
                await unitOfWork.Brands.DeleteAsync(Brand);
                await unitOfWork.CompleteAsync();
                return RedirectToAction("Index");


            }

            return NotFound();
        }

    }
}
