using CoreMart.BLL.Repository.Interface;
using CoreMart.DAL.Context;
using CoreMart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace CoreMart.BLL.Repository.Implementation
{
    public class OrderHeaderRepository : IOrderHeaderRepository
    {
        private readonly CoreMartDbContext _context;
         
        public OrderHeaderRepository(CoreMartDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OrderHeader orderHeader)
        {
            await _context.OrderHeaders.AddAsync(orderHeader);
        }

        public void Delete(OrderHeader orderHeader)
        {
            _context.OrderHeaders.Remove(orderHeader);
        }

        public async Task<IEnumerable<OrderHeader>> GetAllAsync()
        {
            return await _context.OrderHeaders.ToListAsync();
        }

        public async Task<OrderHeader> GetByIdAsync(int id)
        {
            return await _context.OrderHeaders.FirstOrDefaultAsync(p=>p.Id==id);
        }

        public async Task<OrderHeader> GetOrderHeaderWithDetails(int orderHeaderId)
        {
            return await _context.OrderHeaders
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(o => o.Id == orderHeaderId);
        }

        public async Task<IEnumerable<OrderHeader>> GetOrdersByUser(string userId)
        {
            return await _context.OrderHeaders
                .Where(o => o.CustomerId == userId)
                .ToListAsync();
        }

        public void Update(OrderHeader orderHeader)
        {
            _context.OrderHeaders.Update(orderHeader);
        }
        public IEnumerable<OrderHeader> GetAll(Expression<Func<OrderHeader, bool>> filter = null,
                                         Expression<Func<OrderHeader, object>> include = null)
        {
            IQueryable<OrderHeader> query = _context.OrderHeaders;

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = query.Include(include);

            return query.ToList();
        }



        public void UpdateOrderStatus(int id, string orderStatus, string PaymentStatus)
        {
            var OrderFromDb = _context.OrderHeaders.FirstOrDefault(o => o.Id == id);
            if (OrderFromDb != null)
            {
                OrderFromDb.OrderStatus = orderStatus;
                OrderFromDb.PaymentDate=DateTime.Now;   
                if (PaymentStatus!=null)
                {
                    OrderFromDb.paymentStatus = PaymentStatus;
                }
                _context.SaveChanges();
            }
        }
    }
}
