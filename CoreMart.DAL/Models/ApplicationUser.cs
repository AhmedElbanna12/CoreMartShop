using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.DAL.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }




    }
}
