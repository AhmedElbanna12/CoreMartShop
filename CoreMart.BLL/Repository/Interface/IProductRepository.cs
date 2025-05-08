using CoreMart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.BLL.Repository.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task <Product> GetFirstorDefault(int id);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
        void Add(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
    }
}
