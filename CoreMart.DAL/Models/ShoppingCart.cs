using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.DAL.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public virtual ApplicationUser Customer { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }

        public decimal PriceAtThatTime { get; set; }



    }
}
