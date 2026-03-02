using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerGuestAppConciergeItem : Auditable
{
    public CustomerGuestAppConciergeItem()
    {
        GuestRequests = new HashSet<GuestRequest>();
    }

    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public int? CustomerGuestAppConciergeCategoryId { get; set; }
    [MaxLength(200)]
    public string? Name { get; set; }
    [DefaultValue(false)]
    public bool? ItemsMonth { get; set; }
    [DefaultValue(false)]
    public bool? ItemsDay { get; set; }
    [DefaultValue(false)]
    public bool? ItemsMinute { get; set; }
    [DefaultValue(false)]
    public bool? ItemsHour { get; set; }
    [DefaultValue(false)]
    public bool? QuantityBar { get; set; }
    [DefaultValue(false)]
    public bool? ItemLocation { get; set; }
    [DefaultValue(false)]
    public bool? Comment { get; set; }
    [DefaultValue(false)]
    public bool? IsPriceEnable { get; set; }
    public decimal? Price { get; set; }
    [MaxLength(3)]
    public string? Currency { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsPublish { get; set; }
    public string? JsonData { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual CustomerGuestAppBuilder? CustomerGuestAppBuilder { get; set; }
    public virtual CustomerGuestAppConciergeCategory? CustomerGuestAppConciergeCategory { get; set; }
    public virtual ICollection<GuestRequest> GuestRequests { get; set; }
}
