using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CoreMart.DAL.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ValidateNever]
        public Product Product { get; set; }
        [ForeignKey("OrderHeader")]
        public int OrderId { get; set; }

        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
