using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using ProductCatalog.Domain.Products.ViewModels;

namespace ProductCatalog.Infrastructure.Products.Adapters.Formatters
{
	internal class ProductCsvOutputFormatter : TextOutputFormatter
	{
		public ProductCsvOutputFormatter()
		{
			SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));

			SupportedEncodings.Add(Encoding.UTF8);
			SupportedEncodings.Add(Encoding.Unicode);
		}

		protected override bool CanWriteType(Type type)
		{			
			if (typeof(ProductViewModel).IsAssignableFrom(type) 
				|| typeof(IEnumerable<ProductViewModel>).IsAssignableFrom(type))
			{
				return base.CanWriteType(type);
			}
			return false;			
		}
		
		public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
		{
			var response = context.HttpContext.Response;
			var buffer = new StringBuilder();
			
			if (context.Object is IEnumerable<ProductViewModel> products)
			{
				foreach (var product in products)
				{
					AppendLine(buffer, product);
				}
			}
			else
			{
				AppendLine(buffer, context.Object as ProductViewModel);
            }

            var fileName = GetFileName(context.HttpContext.Request);
            var cd = new ContentDisposition
            {
                FileName = fileName,
                Inline = false
            };
            response.Headers.Add("Content-Disposition", cd.ToString());

            return response.WriteAsync(buffer.ToString());
		}

        private string GetFileName(HttpRequest request)
        {
            var searchParams = request.Query.ContainsKey("search") ? request.Query["search"].ToString() : null;
            if (!string.IsNullOrEmpty(searchParams))
            {
                var regexSearch = Path.GetInvalidFileNameChars().ToString();
                var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
                var searchSafeParams = r.Replace(searchParams, "");
                return $"Products({searchSafeParams}).csv";
            }
            return "Products.csv";
        }

        private void AppendLine(StringBuilder buffer, ProductViewModel product)
		{
			if (product == null) 
			{
				buffer.AppendLine();
				return;
			}
			
			buffer.AppendLine($"{GetSafeCellValue(product.Id)},{GetSafeCellValue(product.Code)},{GetSafeCellValue(product.Name)},{GetSafeCellValue(product.Price)},{GetSafeCellValue(product.Photo)},{GetSafeCellValue(product.LastUpdated)}");
		}

		static char[] _specialChars = new char[] { ',', '\n', '\r', '"' };

		private string GetSafeCellValue(object o)
		{
			if (o == null)
			{
				return "";
			}
			string field = o.ToString();
			if (field.IndexOfAny(_specialChars) != -1)
			{
				return String.Format("\"{0}\"", field.Replace("\"", "\"\""));
			}
			else return field;
		}
		
	}
}