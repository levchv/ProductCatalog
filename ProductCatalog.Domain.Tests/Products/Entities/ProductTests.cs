using System;
using System.Linq;
using NUnit.Framework;
using ProductCatalog.Domain.Core.DataModels;
using ProductCatalog.Domain.Products.Entities;

namespace ProductCatalog.Domain.Tests.Products.Entities
{
    public class ProductTests
    {
		[Test]
		public void ClassImplementsIEntityInterface()
		{
			var type = typeof(Product);
			var @interface = typeof(IEntity);
			Assert.IsTrue(type.GetInterfaces().Contains(@interface));
		}
    }
}