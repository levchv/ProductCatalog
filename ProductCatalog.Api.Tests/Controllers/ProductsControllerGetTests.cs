using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ProductCatalog.Api.Controllers;
using ProductCatalog.Domain.Products.Ports.Driving;
using ProductCatalog.Domain.Products.ViewModels;

namespace ProductCatalog.Api.Tests.Controllers
{
    public class ProductsControllerGetTests
    {
		[TestCase("12")]
        public async Task Get_ExistedId_ReturnsOrResultWithProduct(string id)
		{
			var product = GetProduct(id);
			var queriesHandlerMock = new Mock<IProductQueriesHandler>();
			queriesHandlerMock.Setup(handler => handler.GetAsync(id)).ReturnsAsync(product);

			var productController = new ProductsController(GetProductCommandsHandler(), queriesHandlerMock.Object, null);
			var result = await productController.GetAsync(id);

			Assert.IsInstanceOf<OkObjectResult>(result);
			Assert.AreEqual(product, ((OkObjectResult)result).Value);
		}
		
		[TestCase("12")]
        public async Task Get_NotExistedId_ReturnsNotFoundResult(string id)
		{
			var queriesHandlerMock = new Mock<IProductQueriesHandler>();
			queriesHandlerMock.Setup(handler => handler.GetAsync(id)).ReturnsAsync((ProductViewModel)null);

			var productController = new ProductsController(GetProductCommandsHandler(), queriesHandlerMock.Object, null);
			var result = await productController.GetAsync(id);

			Assert.IsInstanceOf<NotFoundResult>(result);
		}
		
		[TestCase("")]
		[TestCase(null)]
        public async Task GetItems_WithEmptySearch_ReturnsOkObjectResultWithExpectedCollection(string search)
		{
			var products = GetProducts();

			var queriesHandlerMock = new Mock<IProductQueriesHandler>();
			queriesHandlerMock.Setup(handler => handler.GetAllAsync()).ReturnsAsync(products);

			var productController = new ProductsController(GetProductCommandsHandler(), queriesHandlerMock.Object, null);
			var result = await productController.GetItemsAsync(search, null);

			Assert.IsInstanceOf<OkObjectResult>(result);
			Assert.AreEqual(products, ((OkObjectResult)result).Value);
		}
		
		[TestCase("test")]
        public async Task GetItems_WithNotEmptySearch_ReturnsOkObjectResultWithExpectedCollection(string search)
		{
			var products = GetProducts();

			var queriesHandlerMock = new Mock<IProductQueriesHandler>();
			queriesHandlerMock.Setup(handler => handler.SearchAsync(search)).ReturnsAsync(products);

			var productController = new ProductsController(GetProductCommandsHandler(), queriesHandlerMock.Object, null);
			var result = await productController.GetItemsAsync(search, null);

			Assert.IsInstanceOf<OkObjectResult>(result);
			Assert.AreEqual(products, ((OkObjectResult)result).Value);
		}

		#region Additional functions

		private IProductCommandsHandler GetProductCommandsHandler()
		{
			var handler = new Mock<IProductCommandsHandler>();
			return handler.Object;
		}

		private ProductViewModel GetProduct(string id)
		{
			return new ProductViewModel(id, "code", "name", 100.0, "photo", DateTime.Now);
		}

		private ProductViewModel[] GetProducts()
		{
			return new ProductViewModel[]
			{
				GetProduct("1"),
				GetProduct("23")
			};
		}

		#endregion
    }
}