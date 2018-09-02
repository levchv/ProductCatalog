using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ProductCatalog.Api.Controllers;
using ProductCatalog.Domain.Core.Ports.Driving.Commands;
using ProductCatalog.Domain.Core.Ports.Driving.Queries;

namespace ProductCatalog.Api.Tests.Controllers
{
    public class ProductsControllerDeleteTests
    {
		[TestCase("25")]
        public async Task Delete_ExistingProduct_ReturnsNoContentResult(string id)
		{
			var commandHandlerMock = new Mock<IProductCommandsHandler>();
			commandHandlerMock.Setup(handler => handler.DeleteAsync(id)).ReturnsAsync(true);

			var productController = new ProductsController(commandHandlerMock.Object, GetProductQueriesHandler(), null);
			var result = await productController.DeleteAsync(id);

			Assert.IsInstanceOf<NoContentResult>(result);
		}
		
		[TestCase("25")]
        public async Task Delete_NotExistingProduct_ReturnsNotFoundResult(string id)
		{
			var commandHandlerMock = new Mock<IProductCommandsHandler>();
			commandHandlerMock.Setup(handler => handler.DeleteAsync(id)).ReturnsAsync(false);

			var productController = new ProductsController(commandHandlerMock.Object, GetProductQueriesHandler(), null);
			var result = await productController.DeleteAsync(id);

			Assert.IsInstanceOf<NotFoundResult>(result);
		}
		
		[TestCase("25")]
        public async Task Delete_OnInnerException_ReturnsStatusCode500(string id)
		{
			var commandHandlerMock = new Mock<IProductCommandsHandler>();
			commandHandlerMock.Setup(handler => handler.DeleteAsync(id)).ThrowsAsync(new Exception());

			var productController = new ProductsController(commandHandlerMock.Object, GetProductQueriesHandler(), null);
			var result = await productController.DeleteAsync(id);

			Assert.IsInstanceOf<StatusCodeResult>(result);
			Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
		}

		#region Additional functions
		
		private IProductQueriesHandler GetProductQueriesHandler()
		{
			var handler = new Mock<IProductQueriesHandler>();
			return handler.Object;
		}

		#endregion
    }
}