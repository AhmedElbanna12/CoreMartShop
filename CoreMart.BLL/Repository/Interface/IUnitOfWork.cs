using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.BLL.Repository.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }

        IShppingCartRepository ShoppingCart { get; }
        IBrandRepository Brands { get; }

        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailsRepository OrderDetails { get; }


        Task<int> CompleteAsync();
    }
}
