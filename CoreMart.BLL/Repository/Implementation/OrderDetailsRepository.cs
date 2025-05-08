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
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        private readonly CoreMartDbContext _context; 

        public OrderDetailsRepository(CoreMartDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OrderDetails orderDetails)
        {
            await _context.OrderDetails.AddAsync(orderDetails);
        }

        public Task DeleteAsync(OrderDetails orderDetails)
        {
            _context.OrderDetails.Remove(orderDetails);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<OrderDetails>> GetAllAsync()
        {
            return await _context.OrderDetails
                .Include(d => d.Product)
                .ToListAsync();
        }

        public async Task<OrderDetails> GetByIdAsync(int id)
        {
            return await _context.OrderDetails.FindAsync(id);
        }

        public async Task<IEnumerable<OrderDetails>> GetOrderDetailsByOrderId(int orderId)
        {
            return await _context.OrderDetails
                .Where(d => d.OrderId == orderId)
                .Include(d => d.Product)
                .ToListAsync();
        }

        public Task UpdateAsync(OrderDetails orderDetails)
        {
            _context.OrderDetails.Update(orderDetails);
            return Task.CompletedTask;
        }
    }
}
