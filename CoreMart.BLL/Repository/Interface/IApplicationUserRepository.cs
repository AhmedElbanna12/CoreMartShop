using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CoreMart.DAL.Models;

namespace CoreMart.BLL.Repository.Interface
{
    public interface IApplicationUserRepository
    {
        ApplicationUser GetFirstOrDefault(Expression<Func<ApplicationUser, bool>> filter);

    }
}
