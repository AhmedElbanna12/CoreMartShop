using CoreMart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.BLL.Repository.Interface
{
    public interface IBrandRepository
    {
        IEnumerable<Brand> GetBrands();
        Task<IEnumerable<Brand>> GetAllAsync();
        Task<Brand> GetByIdAsync(int id);
        Task AddAsync(Brand brand);
        Task UpdateAsync(Brand brand);
        Task DeleteAsync(Brand brand);

    }
}
