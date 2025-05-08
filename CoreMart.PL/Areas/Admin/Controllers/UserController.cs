using System.Security.Claims;
using Azure;
using CoreMart.BLL.Repository.Implementation;
using CoreMart.BLL.Repository.Interface;
using CoreMart.DAL.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreMart.PL.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly CoreMartDbContext context;


        public UserController(CoreMartDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userid = claim.Value;



            return View(context.ApplicationUsers.Where(x=>x.Id !=userid).ToList());
        }


        public async Task<IActionResult> LockUnlock(string ? id)
        {
            var user = await context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd =  DateTime.Now.AddDays(3);
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
            }

            context.SaveChanges();
            return RedirectToAction("Index", "User", new { area = "Admin" });

        }
    }
}
