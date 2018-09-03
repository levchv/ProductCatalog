using Microsoft.AspNetCore.Mvc.Formatters;

namespace ProductCatalog.Infrastructure.Products.Adapters.Formatters
{
    public class ProductFormattersFactory
    {
        public TextOutputFormatter GetCsvOutputFormatter()
		{
			return new ProductCsvOutputFormatter();
		}
    }
}