using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Queries.GetCustomerEnhanceYourStayItems;

public class GetCustomerEnhanceYourStayItemsOut : BaseResponseOut
{
    public GetCustomerEnhanceYourStayItemsOut(string message, List<CustomerEnhanceYourStayItemsOut> customerEnhanceYourStayItemsOut) : base(message)
    {
        CustomersEnhanceYourStayItemsOut = customerEnhanceYourStayItemsOut;
    }
    public List<CustomerEnhanceYourStayItemsOut> CustomersEnhanceYourStayItemsOut { get; set; } = new List<CustomerEnhanceYourStayItemsOut>();
}
public class CustomerEnhanceYourStayItemsOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }

    public int? CustomerGuestAppBuilderCategoryId { get; set; }
    /// <summary>
    /// 1: Newm    2: Best Seller,    3: Recommended,    4: Special,    5: Free,    6: No Badge
    /// </summary>
    public byte? Badge { get; set; }
    public string? ShortDescription { get; set; }
    public string? LongDescription { get; set; }
    /// <summary>
    ///  1: Purchase,    2: Call to Action,    3: Informative
    /// </summary>
    public byte? ButtonType { get; set; }
    public string? ButtonText { get; set; }
    /// <summary>
    ///  1: Per Unit,    2: Per Night,   3: Per Person,    4: Per Person Per Night
    /// </summary>
    public byte? ChargeType { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }

    public int? FilteredCount { get; set; }
    public bool? IsDeleted { get; set; }

}