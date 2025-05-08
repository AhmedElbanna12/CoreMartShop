using CoreMart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.BLL.Repository.Interface
{
    public interface IOrderHeaderRepository
    { 
        Task<OrderHeader> GetOrderHeaderWithDetails(int orderHeaderId);
        Task<IEnumerable<OrderHeader>> GetOrdersByUser(string userId);

        public IEnumerable<OrderHeader> GetAll(Expression<Func<OrderHeader, bool>> filter = null,
                                               Expression<Func<OrderHeader, object>> include = null);

        Task<IEnumerable<OrderHeader>> GetAllAsync();
        Task<OrderHeader> GetByIdAsync(int id);
        Task AddAsync(OrderHeader orderHeader);
        void Update(OrderHeader orderHeader);
        void Delete(OrderHeader orderHeader);

        void UpdateOrderStatus(int id , string orderStatus, string PaymentStatus);

    }
}
