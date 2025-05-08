using CoreMart.BLL.Repository.Implementation;
using CoreMart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderDetails = CoreMart.DAL.Models.OrderDetails;

namespace CoreMart.BLL.Repository.Interface
{
    public interface IOrderDetailsRepository
    {
        Task<IEnumerable<OrderDetails>> GetOrderDetailsByOrderId(int orderId);
        Task<OrderDetails> GetByIdAsync(int id);
        Task AddAsync(OrderDetails orderDetails);
        Task UpdateAsync(OrderDetails orderDetails);
        Task DeleteAsync(OrderDetails orderDetails);

    }
}
