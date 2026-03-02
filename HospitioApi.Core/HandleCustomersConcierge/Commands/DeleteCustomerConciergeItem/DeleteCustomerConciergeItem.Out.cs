using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersConcierge.Commands.DeleteCustomerConciergeItem;

public class DeleteCustomerConciergeItemOut : BaseResponseOut
{
    public DeleteCustomerConciergeItemOut(string message, DeletedCustomerConciergeItemOut deletedCustomerConciergeOut) : base(message)
    {
        DeletedCustomerConciergeOut = deletedCustomerConciergeOut;
    }
    public DeletedCustomerConciergeItemOut DeletedCustomerConciergeOut { get; set; }
}
public class DeletedCustomerConciergeItemOut
{
    public int Id { get; set; }
}
public class DeleteCustomerConciergeItem
{
    public int Id { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? Name { get; set; }
    public bool? ItemsMonth { get; set; }
    public bool? ItemsDay { get; set; }
    public bool? ItemsMinute { get; set; }
    public bool? ItemsHour { get; set; }
    public bool? QuantityBar { get; set; }
    public bool? ItemLocation { get; set; }
    public bool? Comment { get; set; }
    public bool? IsPriceEnable { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsDeleted { get; set; }
    public bool? IsActive { get; set; }
}