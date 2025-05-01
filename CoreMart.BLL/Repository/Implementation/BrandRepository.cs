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
    public class BrandRepository : IBrandRepository
    {
        private readonly CoreMartDbContext _context;

        public BrandRepository(CoreMartDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand> GetByIdAsync(int id)
        {
            return await _context.Brands.FindAsync(id);
        }

        public async Task AddAsync(Brand brand)
        {
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Brand brand)
        {
            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Brand brand)
        {
            var brandindb= await _context.Brands.FirstOrDefaultAsync(p=>p.Id == brand.Id);

            if (brandindb != null)
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
            }
        }

        public IEnumerable<Brand> GetBrands()
        {
            return _context.Brands.ToList();
        }



    }
}
