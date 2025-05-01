using CoreMart.BLL.Repository.Interface;
using CoreMart.DAL.Context;
using CoreMart.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.BLL.Repository.Implementation
{
    public class ShoppingCartRepository : IShppingCartRepository
    {
        private readonly CoreMartDbContext _dbContext;
        private readonly DbSet<ShoppingCart> _dbSet;
        public ShoppingCartRepository(CoreMartDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<ShoppingCart>();
        }
        public void Add(ShoppingCart item)
        {
            _dbContext.ShoppingCarts.Add(item); // أو DbSet المناسب
        }

        public IEnumerable<ShoppingCart> GetAll(Expression<Func<ShoppingCart, bool>>? Cariteria = null, string? Includeword = null)
        {
            IQueryable<ShoppingCart> query = _dbSet;

            // Where()
            if (Cariteria != null)
            {
                query = query.Where(Cariteria);
            }
            // Include()
            if (Includeword != null)
            {
                //_context.Products.Include("Category","Brand","User").toList();
                foreach (var item in Includeword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }

            return query.ToList();
        }

        public ShoppingCart GetFirstorDefault(Expression<Func<ShoppingCart, bool>>? Cariteria = null, string? Includeword = null)
        {
            IQueryable<ShoppingCart> query = _dbSet;

            // Where()
            if (Cariteria != null)
            {
                query = query.Where(Cariteria);
            }
            // Include()
            if (Includeword != null)
            {
                //_context.Products.Include("Category","Brand","User").toList();
                foreach (var item in Includeword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }

            return query.SingleOrDefault();
        }

        public void Remove(ShoppingCart item)
        {
            _dbContext.ShoppingCarts.Remove(item);
        }

        public void RemoveRange(IEnumerable<ShoppingCart> entities)
        {
            _dbContext.ShoppingCarts.RemoveRange(entities);
        }



        public int DecreaseCount(ShoppingCart shoppingCart, int Count)
        {
            shoppingCart.Count -= Count;
            return shoppingCart.Count;
        }

        public int IncreaseCount(ShoppingCart shoppingCart, int Count)
        {
            shoppingCart.Count += Count;
            return shoppingCart.Count;
        }
    }
}
