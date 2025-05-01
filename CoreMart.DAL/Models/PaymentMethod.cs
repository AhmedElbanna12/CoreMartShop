using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.DAL.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string PaymentName { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }



    }
}
