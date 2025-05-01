using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.DAL.Models
{
    public class Shipping
    {
        public int Id { get; set; }
        public string ShippingStatus { get; set; }
        public DateTime ShippingDate { get; set; }
        public string TrackingNumber { get; set; }
        public string ShippingAddress { get; set; }
        public decimal ShippingCost { get; set; }

        [ForeignKey("ShoppingCart")]
        public int CartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }



    }
}
