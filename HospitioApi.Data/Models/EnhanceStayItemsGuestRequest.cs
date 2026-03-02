using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Data.Models;

public class EnhanceStayItemsGuestRequest : Auditable
{
    public int CustomerId { get; set; }
    public int GuestId { get; set; }
    public int CustomerGuestAppEnhanceYourStayItemId { get; set; }
    public int? Qty{ get; set; }
    public string? GRPaymentId { get; set; }
    public string? GRPaymentDetails { get; set; }
    /// <summary>
    /// Status can be 1= Completed, 2= In Progress, 3=Will User, 4=Canceled, 5=Rejected etc
    /// </summary>
    public byte? Status { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual CustomerGuest? Guest { get; set; }
    public virtual CustomerGuestAppEnhanceYourStayItem? CustomerGuestAppEnhanceYourStayItem { get; set; }
    public virtual ICollection<EnhanceStayItemExtraGuestRequest>? EnhanceStayItemExtraGuestRequest { get; set; }
}
