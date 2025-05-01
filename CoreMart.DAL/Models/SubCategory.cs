using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CoreMart.DAL.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        public string ImageURL { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }



    }
}
