using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ProductCatalog.Api.Controllers;
using ProductCatalog.Domain.Products.InputModels;
using ProductCatalog.Domain.Products.Ports.Driving;

namespace ProductCatalog.Api.Tests.Controllers
{
    public class ProductsControllerPostTests
    {   
		[TestCase(true)]
		[TestCase(false)]
        public async Task Post_NotValidModel_ReturnsBadRequestResult(bool possibleCommandResult)
		{
			var product = GetProductInputModel();
			var commandHandlerMock = new Mock<IProductCommandsHandler>();
			//string outId = null;
			commandHandlerMock.Setup(handler => handler.CreateAsync(product)).ReturnsAsync(possibleCommandResult);

			var productController = new ProductsController(commandHandlerMock.Object, GetProductQueriesHandler(), null);
			productController.ModelState.AddModelError("test", "test");
			var result = await productController.PostAsync(product);

			Assert.IsInstanceOf<BadRequestResult>(result);
		}

		[Test]
        public async Task Post_ValidProduct_ReturnsObjectResultWithStatusCode201AndId()
		{
			var product = GetProductInputModel();
			var commandHandlerMock = new Mock<IProductCommandsHandler>();
			//string outId = "52";
			commandHandlerMock.Setup(handler => handler.CreateAsync(product)).ReturnsAsync(true);

			var productController = new ProductsController(commandHandlerMock.Object, GetProductQueriesHandler(), null);
			productController.ModelState.Clear();
			var result = await productController.PostAsync(product);

			Assert.IsInstanceOf<StatusCodeResult>(result);			
			Assert.AreEqual(201, ((StatusCodeResult)result).StatusCode);
			//Assert.AreEqual(outId, ((ObjectResult)result).Value);
		}

		[Test]
        public async Task Post_ProductWithNotUniqueCode_ReturnsBadRequestObjectResultWithTrue()
		{
			var product = GetProductInputModel();
			var commandHandlerMock = new Mock<IProductCommandsHandler>();
			//string outId = null;
			commandHandlerMock.Setup(handler => handler.CreateAsync(product)).ReturnsAsync(false);

			var productController = new ProductsController(commandHandlerMock.Object, GetProductQueriesHandler(), null);
			productController.ModelState.Clear();
			var result = await productController.PostAsync(product);

			Assert.IsInstanceOf<BadRequestObjectResult>(result);
			Assert.AreEqual(true, ((BadRequestObjectResult)result).Value);
		}

		[Test]
        public async Task Post_OnInnerException_ReturnsStatusCode500()
		{
			var product = GetProductInputModel();
			var commandHandlerMock = new Mock<IProductCommandsHandler>();
			//string outId = null;
			commandHandlerMock.Setup(handler => handler.CreateAsync(product)).ThrowsAsync(new Exception());

			var productController = new ProductsController(commandHandlerMock.Object, GetProductQueriesHandler(), null);
			productController.ModelState.Clear();
			var result = await productController.PostAsync(product);

			Assert.IsInstanceOf<StatusCodeResult>(result);
			Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
		}
		
		#region Additional functions
		
		private IProductQueriesHandler GetProductQueriesHandler()
		{
			var handler = new Mock<IProductQueriesHandler>();
			return handler.Object;
		}

		private ProductInputModel GetProductInputModel()
		{
			return new ProductInputModel();
		}

		#endregion
    }
}