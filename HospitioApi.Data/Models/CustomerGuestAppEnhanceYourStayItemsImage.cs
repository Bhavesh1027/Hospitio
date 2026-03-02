using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerGuestAppEnhanceYourStayItemsImage : Auditable
{
    public int? CustomerGuestAppEnhanceYourStayItemId { get; set; }
    [MaxLength(500)]
    public string? ItemsImages { get; set; }
    public int? DisaplayOrder { get; set; }
    public string? JsonData { get; set; }
    public bool? IsPublish { get; set; }

    public virtual CustomerGuestAppEnhanceYourStayItem? CustomerGuestAppEnhanceYourStayItem { get; set; }
}
