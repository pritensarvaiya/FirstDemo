using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Models
{
    public class Product
    { 
        public int Id { get; set; }
        [DisplayName("Product Name")]
        public string Name { get; set; } = string.Empty;
        [DisplayName("Description")]
        public string Description { get; set; }= string.Empty;
        [DisplayName("Product Price")]
        public double Price { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
    }
}
