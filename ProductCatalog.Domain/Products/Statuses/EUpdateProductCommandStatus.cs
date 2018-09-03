namespace ProductCatalog.Domain.Products.Statuses
{
    public enum EUpdateProductCommandStatus
    {
        Success = 1,
		ProductNotExists = 2,
		FailsBecauseDuplicatedCode = 3
    }
}