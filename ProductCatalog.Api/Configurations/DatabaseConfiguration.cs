namespace ProductCatalog.Api.Configurations
{
    public class DatabaseConfiguration
    {
		public string ConnectionString { get; set; }
		public bool AutoMigrations { get; set; }
    }
}