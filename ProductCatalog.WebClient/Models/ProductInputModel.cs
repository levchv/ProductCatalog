using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.WebClient.Models
{
    public class ProductInputModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(Double.Epsilon, double.MaxValue)]
        public double Price { get; set; }
        [Url]
        public string Photo { get; set; }
    }
}
