namespace ProductCatalog.Domain.Products.Statuses
{
    public enum EUpdateProductCommandStatus
    {
        Success = 1,
		TargetNotExists = 2,
		FailsBecauseDuplicatedCode = 3
    }
}