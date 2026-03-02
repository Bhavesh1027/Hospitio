using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.CreateCustomerEnhanceYourStayItem;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.UpdateCustomerEnhanceYourStayItem;

public class UpdateCustomerEnhanceYourStayItemOut : BaseResponseOut
{
    public UpdateCustomerEnhanceYourStayItemOut(string message, UpdatedCustomerEnhanceYourStayItemOut updatedCustomerEnhanceYourStayItemOut) : base(message)
    {
        UpdatedCustomerEnhanceYourStayItemOut = updatedCustomerEnhanceYourStayItemOut;
    }
    public UpdatedCustomerEnhanceYourStayItemOut UpdatedCustomerEnhanceYourStayItemOut { get; set; }
}
public class UpdatedCustomerEnhanceYourStayItemOut
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
    public int? DisplayOrder { get; set; }
    public List<UpdatedEnhanceYourStayCategoryItemExtraOut> CustomerEnhanceYourStayCategoryItemsExtra { get; set; } = new List<UpdatedEnhanceYourStayCategoryItemExtraOut>();
    public List<UpdatedEnhanceYourStayItemImageOut> ItemsImages { get; set; } = new List<UpdatedEnhanceYourStayItemImageOut>();

}

public class UpdatedEnhanceYourStayItemImageOut
{
    public int Id { get; set; }
    public int? CustomerGuestAppEnhanceYourStayItemId { get; set; }
    public string? ItemsImages { get; set; }
    public int? DisaplayOrder { get; set; }
}

public class UpdatedEnhanceYourStayCategoryItemExtraOut
{
    public int? Id { get; set; }
    public byte? QueType { get; set; }
    public string? Questions { get; set; }
    public string? OptionValues { get; set; }
}