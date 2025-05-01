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
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await unitOfWork.Categories.GetAllAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadPath = Path.Combine(RootPath, @"Images\Categories");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    var ext = Path.GetExtension(file.FileName);

                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName + ext), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    category.ImageURL = @"Images\Categories\" + fileName + ext;
                }

                await unitOfWork.Categories.AddAsync(category);
                await unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }

            
            return View("Create", category);
        }


        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            var categoryInDB = await unitOfWork.Categories.GetByIdAsync(id);
            if (categoryInDB != null)
            {
                return View(categoryInDB);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadPath = Path.Combine(RootPath, @"Images\Categories");
                    var ext = Path.GetExtension(file.FileName);
                    var newImagePath = Path.Combine(uploadPath, fileName + ext);

                    // حذف الصورة القديمة إذا كانت موجودة
                    if (category.ImageURL != null)
                    {
                        var oldImagePath = Path.Combine(RootPath, category.ImageURL.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(newImagePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }



                    category.ImageURL = @"Images\Categories\" + fileName + ext;

                }

                await unitOfWork.Categories.UpdateAsync(category);
                await unitOfWork.CompleteAsync();
                return RedirectToAction("Index");

            }
            return View(category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var Category = await unitOfWork.Categories.GetByIdAsync(id);
            if (Category != null)
            {
await unitOfWork.Categories.DeleteAsync(Category);
            await unitOfWork.CompleteAsync();
            return RedirectToAction("Index");

                
            }

            return NotFound();
        }


    }
}
