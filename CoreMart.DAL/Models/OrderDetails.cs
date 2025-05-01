using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.DAL.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [ForeignKey("OrderHeader")]
        public int OrderId { get; set; } // fo
        public OrderHeader OrderHeader { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
