using HospitioApi.Core.HandleGuestAppEnhanceYourStayItemImage.Commands.CreateGuestAppEnhanceYourStayItemsImages;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.CreateCustomerEnhanceYourStayItem;

public class CreateCustomerEnhanceYourStayItemIn
{
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }

    public int? CustomerGuestAppBuilderCategoryId { get; set; }
    /// <summary>
    /// 1: New    2: Best Seller,    3: Recommended,    4: Special,    5: Free,    6: No Badge
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
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    public List<CreateEnhanceYourStayItemAttachementIn> ItemsImages { get; set; } = new List<CreateEnhanceYourStayItemAttachementIn>();
    public List<CreateEnhanceYourStayCategoryItemExtraIn> CustomerEnhanceYourStayCategoryItemExtra { get; set; } = new List<CreateEnhanceYourStayCategoryItemExtraIn>();
}

public class CreateEnhanceYourStayItemAttachementIn
{
    public string? ItemsImage { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsDeleted { get; set; }
}

public class CreateEnhanceYourStayCategoryItemExtraIn
{
    public byte? QueType { get; set; }
    public string? Questions { get; set; }
    public string? OptionValues { get; set; }
    public bool? IsDeleted { get; set; }
}