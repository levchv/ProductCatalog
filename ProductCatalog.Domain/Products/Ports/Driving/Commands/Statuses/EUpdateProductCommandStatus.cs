namespace ProductCatalog.Domain.Products.Ports.Driving.Commands.Statuses
{
    public enum EUpdateProductCommandStatus
    {
        Success = 1,
		TargetNotExists = 2,
		FailsBecauseDuplicatedCode = 3
    }
}