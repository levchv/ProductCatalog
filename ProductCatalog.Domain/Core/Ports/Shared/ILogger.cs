using System;

namespace ProductCatalog.Domain.Core.Ports.Shared
{
    public interface ILogger
    {
		void LogException(Exception exp);
		void LogInfo(string info);
	}
}