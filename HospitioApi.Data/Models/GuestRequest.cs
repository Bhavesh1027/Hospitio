using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class GuestRequest : Auditable
{
    public int? CustomerId { get; set; }
    /// <summary>
    ///  1: Enhance Your Stay, 2: Reception, 3: Housekeeping, 4: Room Service, 5: Concierge
    /// </summary>
    public byte? RequestType { get; set; }
    public int? CustomerGuestAppConciergeItemId { get; set; }
    public int? CustomerGuestAppEnhanceYourStayItemId { get; set; }
    public int? CustomerGuestAppHousekeepingItemId { get; set; }
    public int? CustomerGuestAppRoomServiceItemId { get; set; }
    public int? CustomerGuestAppReceptionItemId { get; set; }
    public int? GuestId { get; set; }
    public int? MonthValue { get; set; }
    public int? DayValue { get; set; }
    public int? YearValue { get; set; }
    public int? HourValue { get; set; }
    public int? MinuteValue { get; set; }
    [MaxLength(250)]
    public string? PickupLocation { get; set; }
    [MaxLength(250)]
    public string? Destination { get; set; }
    public string? Comment { get; set; }
    [MaxLength(100)]
    public string? GRPaymentId { get; set; }
    public string? GRPaymentDetails { get; set; }
    /// <summary>
    /// Status can be 1= Completed, 2= In Progress, 3=Will User, 4=Canceled, 5=Rejected etc
    /// </summary>
    public byte? Status { get; set; }
    public int? QuantityBar { get; set; }


    public virtual Customer? Customer { get; set; }
    public virtual CustomerGuestAppConciergeItem? CustomerGuestAppConciergeItem { get; set; }
    public virtual CustomerGuestAppEnhanceYourStayItem? CustomerGuestAppEnhanceYourStayItem { get; set; }
    public virtual CustomerGuestAppHousekeepingItem? CustomerGuestAppHousekeepingItem { get; set; }
    public virtual CustomerGuestAppReceptionItem? CustomerGuestAppReceptionItem { get; set; }
    public virtual CustomerGuestAppRoomServiceItem? CustomerGuestAppRoomServiceItem { get; set; }
    public virtual CustomerGuest? Guest { get; set; }

    public virtual ICollection<ChannelMessages>? ChannelMessages { get; set; }

}
