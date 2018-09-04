using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.WebClient.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Photo { get; set; }
        [DisplayName("Last updated")]
        public DateTime LastUpdated { get; set; }
    }
}
