using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.WebClient.Models
{
    public class ProductsViewModel
    {
        public string Search { get; set; }
        public string ExcelLink { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
