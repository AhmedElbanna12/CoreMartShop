using CoreMart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.BLL.Repository.Interface
{
    public interface IShppingCartRepository
    {
        IEnumerable<ShoppingCart> GetAll(Expression<Func<ShoppingCart, bool>>? Cariteria = null, string? Includeword = null);
        ShoppingCart GetFirstorDefault(Expression<Func<ShoppingCart, bool>>? Cariteria = null, string? Includeword = null);
        void Add(ShoppingCart item);
        void Remove(ShoppingCart item);
        void RemoveRange(IEnumerable<ShoppingCart> entities);

        int IncreaseCount(ShoppingCart shoppingCart, int Count);
        int DecreaseCount(ShoppingCart shoppingCart, int Count);
    }
}
