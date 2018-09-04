using ProductCatalog.Domain.Core.Ports.Shared;

namespace ProductCatalog.Infrastructure.Shared.Adapters.Loggers
{
    public class LoggerFactory
    {
        public ILogger Get()
		{
			return new NoneLogger();
		}
    }
}