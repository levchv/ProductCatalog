using System;
using System.Linq;
using NUnit.Framework;
using ProductCatalog.Domain.Core.DataModels;
using ProductCatalog.Domain.Products.Ports.Driving.Commands.InputModels;

namespace ProductCatalog.Domain.Tests.Products.InputModels
{
    public class ProductInputModelTests
    {
		[Test]
		public void ClassImplementsIEntityInterface()
		{
			var type = typeof(ProductInputModel);
			var @interface = typeof(IInputModel);
			Assert.IsTrue(type.GetInterfaces().Contains(@interface));
		}

		[Test]
		public void ClassHasParameterlessConstuctor()
		{
			var type = typeof(ProductInputModel);
			var constructor = type.GetConstructor(Type.EmptyTypes);
			Assert.IsNotNull(constructor);
		}
        
		[Test]
		public void ClassCouldBeConstruct()
		{
			Assert.DoesNotThrow(() => new ProductInputModel());
		}
    }
}