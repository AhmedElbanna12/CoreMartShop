using CoreMart.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.DAL.Context
{
    public class CoreMartDbContext : IdentityDbContext<ApplicationUser>
    {
        public CoreMartDbContext(DbContextOptions<CoreMartDbContext> options) : base(options) { }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }









    }
}
