using CoreMart.BLL.Repository.Interface;
using CoreMart.DAL.Context;
using CoreMart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace CoreMart.BLL.Repository.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly CoreMartDbContext _context;

        public ProductRepository(CoreMartDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Include(p => p.Brand).Include(p => p.Category).ToListAsync();
        }

        public async Task<Product> GetById(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(p => p.Id == id); // Blocking call
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.Products.Include(p => p.Category).Include(p => p.Brand)
                                 .Where(p => p.CategoryId == categoryId)
                                 .ToListAsync();
        }

        public void Add(Product product)
        {
             _context.Products.Add(product);
           
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
         
        }
        public async Task DeleteAsync(Product product)
        {
            var productindb = await _context.Products.FirstOrDefaultAsync(p=>p.Id == product.Id);
            if (productindb != null)
            {
                _context.Products.Remove(product);
            }
        }
    }
}
