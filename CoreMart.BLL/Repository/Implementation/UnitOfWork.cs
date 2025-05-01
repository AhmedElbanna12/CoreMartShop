using CoreMart.BLL.Repository.Interface;
using CoreMart.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.BLL.Repository.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoreMartDbContext _context;

        public UnitOfWork(CoreMartDbContext context)
        //  IShoppingCartRepository shoppingCartRepository)
        {
            _context = context;
            Categories = new CategoryRepository(_context);
            Products = new ProductRepository(_context);
            Brands = new BrandRepository(_context);
            ShoppingCart = new ShoppingCartRepository(_context);
            OrderDetails = new OrderDetailsRepository(_context); 
            OrderHeader = new OrderHeaderRepository(_context);
        }

        public ICategoryRepository Categories { get; private set; }
        public IProductRepository Products { get; private set; }
        public IBrandRepository Brands { get; private set; }
        public IShppingCartRepository ShoppingCart { get; private set; }

        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }



        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
