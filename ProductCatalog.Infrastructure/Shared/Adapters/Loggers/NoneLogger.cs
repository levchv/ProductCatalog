using System;
using ProductCatalog.Domain.Core.Ports.Shared;

namespace ProductCatalog.Infrastructure.Shared.Adapters.Loggers
{
	internal class NoneLogger : ILogger
	{
		public void LogException(Exception exp)
		{
		}
	}
}