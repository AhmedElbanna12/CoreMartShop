using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CoreMart.BLL.Repository.Interface;
using CoreMart.DAL.Context;
using CoreMart.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreMart.BLL.Repository.Implementation
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly CoreMartDbContext _context;

        public ApplicationUserRepository(CoreMartDbContext context)
        {
            _context = context;
        }
        public ApplicationUser GetFirstOrDefault(Expression<Func<ApplicationUser, bool>> filter)
        {
            return _context.ApplicationUsers.FirstOrDefault(filter);
        }

    }
}
