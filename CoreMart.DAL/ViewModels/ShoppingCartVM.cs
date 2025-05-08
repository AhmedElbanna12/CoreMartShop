using CoreMart.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.DAL.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> CartList { get; set; }
        [ForeignKey("OrderHeader")]
        public int OrderHeaderId { get; set; }
        public OrderHeader OrderHeader { get; set; }

        public decimal TotalCarts { get; set; }


        public Product product { get; set; }



        [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100.")]
        public int Count { get; set; }
    }
}
