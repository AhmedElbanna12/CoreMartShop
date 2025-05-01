using CoreMart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.BLL.Repository.Interface
{
    public interface IOrderHeaderRepository
    { 
        // نقدر نضيف ميثودز إضافية إذا احتاجنا مثلاً
        Task<OrderHeader> GetOrderHeaderWithDetails(int orderHeaderId);
        Task<IEnumerable<OrderHeader>> GetOrdersByUser(string userId);

       
        Task<IEnumerable<OrderHeader>> GetAllAsync();
        Task<OrderHeader> GetByIdAsync(int id);
        Task AddAsync(OrderHeader orderHeader);
        void Update(OrderHeader orderHeader);
        void Delete(OrderHeader orderHeader);

    }
}
