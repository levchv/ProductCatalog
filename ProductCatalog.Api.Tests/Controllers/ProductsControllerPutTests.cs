using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ProductCatalog.Api.Controllers;
using ProductCatalog.Domain.Core.Ports.Driving.Commands;
using ProductCatalog.Domain.Core.Ports.Driving.Queries;
using ProductCatalog.Domain.Products.Ports.Driving.Commands.InputModels;
using ProductCatalog.Domain.Products.Ports.Driving.Commands.Statuses;

namespace ProductCatalog.Api.Tests.Controllers
{
    public class ProductsControllerPutTests
    {            
		[TestCase("25", EUpdateProductCommandStatus.Success)]
		[TestCase("25", EUpdateProductCommandStatus.TargetNotExists)]
		[TestCase("25", EUpdateProductCommandStatus.FailsBecauseDuplicatedCode)]
        public async Task Put_NotValidModel_ReturnsBadRequestResult(string id, EUpdateProductCommandStatus possibleCommandResult)
		{
			var product = GetProductInputModel();
			var commandHandlerMock = new Mock<IProductCommandsHandler>();
			commandHandlerMock.Setup(handler => handler.UpdateAsync(id, product)).ReturnsAsync(possibleCommandResult);

			var productController = new ProductsController(commandHandlerMock.Object, GetProductQueriesHandler(), null);
			productController.ModelState.AddModelError("test", "test");
			var result = await productController.PutAsync(id, product);

			Assert.IsInstanceOf<BadRequestResult>(result);
		}

		[TestCase("25")]
        public async Task Put_ValidProduct_ReturnsOkResult(string id)
		{
			var product = GetProductInputModel();
			var commandHandlerMock = new Mock<IProductCommandsHandler>();
			commandHandlerMock.Setup(handler => handler.UpdateAsync(id, product)).ReturnsAsync(EUpdateProductCommandStatus.Success);

			var productController = new ProductsController(commandHandlerMock.Object, GetProductQueriesHandler(), null);
			productController.ModelState.Clear();
			var result = await productController.PutAsync(id, product);

			Assert.IsInstanceOf<OkResult>(result);
		}

		[TestCase("25")]
        public async Task Put_NotExistingProduct_ReturnsNotFoundResult(string id)
		{
			var product = GetProductInputModel();
			var commandHandlerMock = new Mock<IProductCommandsHandler>();
			commandHandlerMock.Setup(handler => handler.UpdateAsync(id, product)).ReturnsAsync(EUpdateProductCommandStatus.TargetNotExists);

			var productController = new ProductsController(commandHandlerMock.Object, GetProductQueriesHandler(), null);
			productController.ModelState.Clear();
			var result = await productController.PutAsync(id, product);

			Assert.IsInstanceOf<NotFoundResult>(result);
		}

		[TestCase("25")]
        public async Task Put_ProducWithDuplicatedCode_ReturnsBadRequestObjectResultWithFailCode(string id)
		{
			var product = GetProductInputModel();
			var commandHandlerMock = new Mock<IProductCommandsHandler>();
			commandHandlerMock.Setup(handler => handler.UpdateAsync(id, product)).ReturnsAsync(EUpdateProductCommandStatus.FailsBecauseDuplicatedCode);

			var productController = new ProductsController(commandHandlerMock.Object, GetProductQueriesHandler(), null);
			productController.ModelState.Clear();
			var result = await productController.PutAsync(id, product);

			Assert.IsInstanceOf<BadRequestObjectResult>(result);
			Assert.AreEqual((int)EUpdateProductCommandStatus.FailsBecauseDuplicatedCode, ((BadRequestObjectResult)result).Value);
		}

		[TestCase("25")]
        public async Task Put_OnInnerException_ReturnsStatusCode500(string id)
		{
			var product = GetProductInputModel();
			var commandHandlerMock = new Mock<IProductCommandsHandler>();
			commandHandlerMock.Setup(handler => handler.UpdateAsync(id, product)).ThrowsAsync(new Exception());

			var productController = new ProductsController(commandHandlerMock.Object, GetProductQueriesHandler(), null);
			productController.ModelState.Clear();
			var result = await productController.PutAsync(id, product);

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