using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.DAL.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }  
        [ForeignKey("ApplicationUser")]
        public string CustomerId { get; set; } 

        public ApplicationUser ApplicationUser { get; set; }

        public DateTime OrderDate { get; set; } 
        public decimal TotalAmount { get; set; }
        public string? OrderStatus { get; set; } 

        public DateTime ShippingDate { get; set; }

        public string? paymentStatus { get; set; }

        public string? TrackingNumber { get; set; }

        public string ? Carrier { get; set; }

        public DateTime PaymentDate { get; set; }



        // Strip Properities 
        public string? SessionId { get; set; }

        public string ? PaymentIntendId { get; set; }


        //Data of User

        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }
    }

}
