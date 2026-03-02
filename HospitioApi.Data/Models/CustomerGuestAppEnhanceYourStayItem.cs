using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerGuestAppEnhanceYourStayItem : Auditable
{
    public CustomerGuestAppEnhanceYourStayItem()
    {
        CustomerGuestAppEnhanceYourStayCategoryItemsExtras = new HashSet<CustomerGuestAppEnhanceYourStayCategoryItemsExtra>();
        CustomerGuestAppEnhanceYourStayItemsImages = new HashSet<CustomerGuestAppEnhanceYourStayItemsImage>();
        GuestRequests = new HashSet<GuestRequest>();
    }


    public int? CustomerGuestAppBuilderId { get; set; }
    public int? CustomerId { get; set; }
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
    [MaxLength(12)]
    public string? ButtonText { get; set; }
    /// <summary>
    ///  1: Per Unit,    2: Per Night,   3: Per Person,    4: Per Person Per Night
    /// </summary>
    public byte? ChargeType { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Price { get; set; }
    [MaxLength(3)]
    public string? Currency { get; set; }
    public int? DisplayOrder { get; set; }
    public string? JsonData { get; set; }
    public bool? IsPublish { get; set; }

    public virtual Customer? Customer { get; set; }
    public virtual CustomerGuestAppBuilder? CustomerGuestAppBuilder { get; set; }
    public virtual CustomerGuestAppEnhanceYourStayCategory? CustomerGuestAppBuilderCategory { get; set; }
    public virtual ICollection<CustomerGuestAppEnhanceYourStayCategoryItemsExtra> CustomerGuestAppEnhanceYourStayCategoryItemsExtras { get; set; }
    public virtual ICollection<CustomerGuestAppEnhanceYourStayItemsImage> CustomerGuestAppEnhanceYourStayItemsImages { get; set; }
    public virtual ICollection<GuestRequest> GuestRequests { get; set; }
}
