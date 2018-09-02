using System;
using Moq;
using NUnit.Framework;
using ProductCatalog.Api.Controllers;
using ProductCatalog.Domain.Core.Ports.Driving.Commands;
using ProductCatalog.Domain.Core.Ports.Driving.Queries;
using ProductCatalog.Domain.Core.Ports.Shared;

namespace ProductCatalog.Api.Tests.Controllers
{
    public class ProductsControllerTests
    {           
		[Test]
		public void ClassCouldBeConstruct()
		{
			var commandsHandler = GetProductCommandsHandler();
			var queriesHandler = GetProductQueriesHandler();
			var logger = GetLogger();
			Assert.DoesNotThrow(() => new ProductsController(commandsHandler, queriesHandler, logger));
		}

		[Test]
		public void ConstructorRequiresProductCommandsHandler()
		{
			var queriesHandler = GetProductQueriesHandler();
			var logger = GetLogger();			
			Assert.Throws<ArgumentNullException>(() => new ProductsController(null, queriesHandler, logger));
		}

		[Test]
		public void ConstructorRequiresProductQueriesHandler()
		{
			var commandsHandler = GetProductCommandsHandler();
			var logger = GetLogger();			
			Assert.Throws<ArgumentNullException>(() => new ProductsController(commandsHandler, null, logger));
		}

		[Test]
		public void ConstructorNotRequiresLogger()
		{		
			var commandsHandler = GetProductCommandsHandler();
			var queriesHandler = GetProductQueriesHandler();
			Assert.DoesNotThrow(() => new ProductsController(commandsHandler, queriesHandler, null));
		}

		#region Additional functions

		private IProductCommandsHandler GetProductCommandsHandler()
		{
			var handler = new Mock<IProductCommandsHandler>();
			return handler.Object;
		}
		
		private IProductQueriesHandler GetProductQueriesHandler()
		{
			var handler = new Mock<IProductQueriesHandler>();
			return handler.Object;
		}

		private ILogger GetLogger()
		{
			var loggerMock = new Mock<ILogger>();
			return loggerMock.Object;
		}

		#endregion
    }
}