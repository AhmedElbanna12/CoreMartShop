using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.DAL.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public virtual Category Category { get; set; }



        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        [ValidateNever]
        public Brand Brand { get; set; }

        [ValidateNever]
        public string ImageURL { get; set; }


    }
}
