using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerGuestAppBuilder : Auditable
{
    public CustomerGuestAppBuilder()
    {
        CustomerGuestAppConciergeCategories = new HashSet<CustomerGuestAppConciergeCategory>();
        CustomerGuestAppConciergeItems = new HashSet<CustomerGuestAppConciergeItem>();
        CustomerGuestAppEnhanceYourStayCategories = new HashSet<CustomerGuestAppEnhanceYourStayCategory>();
        CustomerGuestAppEnhanceYourStayItems = new HashSet<CustomerGuestAppEnhanceYourStayItem>();
        CustomerGuestAppHousekeepingCategories = new HashSet<CustomerGuestAppHousekeepingCategory>();
        CustomerGuestAppHousekeepingItems = new HashSet<CustomerGuestAppHousekeepingItem>();
        CustomerGuestAppReceptionCategories = new HashSet<CustomerGuestAppReceptionCategory>();
        CustomerGuestAppReceptionItems = new HashSet<CustomerGuestAppReceptionItem>();
        CustomerGuestAppRoomServiceCategories = new HashSet<CustomerGuestAppRoomServiceCategory>();
        CustomerGuestAppRoomServiceItems = new HashSet<CustomerGuestAppRoomServiceItem>();
        CustomerPropertyInformations = new HashSet<CustomerPropertyInformation>();
    }


    public int? CustomerId { get; set; }
    public int? CustomerRoomNameId { get; set; }
    [MaxLength(150)]
    public string? Message { get; set; }
    [MaxLength(150)]
    public string? SecondaryMessage { get; set; }
    public bool? LocalExperience { get; set; }
    public bool? Ekey { get; set; }
    public bool? PropertyInfo { get; set; }
    public bool? EnhanceYourStay { get; set; }
    public bool? Reception { get; set; }
    public bool? Housekeeping { get; set; }
    public bool? RoomService { get; set; }
    public bool? Concierge { get; set; }
    public bool? TransferServices { get; set; }
    public bool? OnlineCheckIn { get; set; }
    /// <summary>
    /// 1=Completed, 2=In Process, 3=NeedToDO
    /// </summary>
    public byte? IsWork { get; set; }
    public bool? IsPublish { get; set; }
    public string? JsonData { get; set; }

    public virtual Customer? Customer { get; set; }
    public virtual CustomerRoomName? CustomerRoomName { get; set; }
    public virtual ICollection<CustomerGuestAppConciergeCategory> CustomerGuestAppConciergeCategories { get; set; }
    public virtual ICollection<CustomerGuestAppConciergeItem> CustomerGuestAppConciergeItems { get; set; }
    public virtual ICollection<CustomerGuestAppEnhanceYourStayCategory> CustomerGuestAppEnhanceYourStayCategories { get; set; }
    public virtual ICollection<CustomerGuestAppEnhanceYourStayItem> CustomerGuestAppEnhanceYourStayItems { get; set; }
    public virtual ICollection<CustomerGuestAppHousekeepingCategory> CustomerGuestAppHousekeepingCategories { get; set; }
    public virtual ICollection<CustomerGuestAppHousekeepingItem> CustomerGuestAppHousekeepingItems { get; set; }
    public virtual ICollection<CustomerGuestAppReceptionCategory> CustomerGuestAppReceptionCategories { get; set; }
    public virtual ICollection<CustomerGuestAppReceptionItem> CustomerGuestAppReceptionItems { get; set; }
    public virtual ICollection<CustomerGuestAppRoomServiceCategory> CustomerGuestAppRoomServiceCategories { get; set; }
    public virtual ICollection<CustomerGuestAppRoomServiceItem> CustomerGuestAppRoomServiceItems { get; set; }
    public virtual ICollection<CustomerPropertyInformation> CustomerPropertyInformations { get; set; }
}
