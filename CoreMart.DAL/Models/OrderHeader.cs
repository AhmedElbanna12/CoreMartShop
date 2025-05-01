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
        public int Id { get; set; } // معرف الطلب
        [ForeignKey("ApplicationUser")]
        public string CustomerId { get; set; } // معرف المستخدم

        public ApplicationUser ApplicationUser { get; set; }

        public DateTime OrderDate { get; set; } // تاريخ الطلب
        public decimal TotalAmount { get; set; } // المجموع الكلي للطلب
        public string Status { get; set; } // حالة الطلب (مثال: "Pending", "Completed")


        // العلاقة بين الـ OrderHeader والـ OrderDetail (واحد إلى متعدد)
        public ICollection<OrderDetails> OrderDetails { get; set; }
    }

}
