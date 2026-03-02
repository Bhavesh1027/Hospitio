
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Data.Models;

public class EnhanceStayItemExtraGuestRequest : Auditable
{
    public int EnhanceStayItemsGuestRequestId { get; set; }
    public int CustomerGuestAppEnhanceYourStayCategoryItemsExtraId { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public int? Year { get; set; }
    public int? Hour { get; set; }
    public int? Minute { get; set; }
    public string? PickupLocation { get; set; }
    public int? Qunatity { get; set; }
    public string? Destination { get; set; }
    public string? Comment { get; set; }
    public byte? Status { get; set; }
    public virtual EnhanceStayItemsGuestRequest? EnhanceStayItemsGuestRequest { get; set; }
    public virtual CustomerGuestAppEnhanceYourStayCategoryItemsExtra? CustomerGuestAppEnhanceYourStayCategoryItemsExtra { get;set; }

}
