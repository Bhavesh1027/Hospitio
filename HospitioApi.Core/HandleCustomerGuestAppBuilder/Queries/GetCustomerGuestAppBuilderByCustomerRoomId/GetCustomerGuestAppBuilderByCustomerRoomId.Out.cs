using HospitioApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerGuestAppBuilderByCustomerRoomId;

public class GetCustomerGuestAppBuilderByCustomerRoomIdOut : BaseResponseOut
{
    public GetCustomerGuestAppBuilderByCustomerRoomIdOut(string message, CustomerGuestAppBuilderByCustomerRoomIdOut customerGuestAppBuilderByCustomerRoomIdOut) : base(message)
    {
        CustomerGuestAppBuilderByCustomerRoomIdOut = customerGuestAppBuilderByCustomerRoomIdOut;
    }
    public CustomerGuestAppBuilderByCustomerRoomIdOut CustomerGuestAppBuilderByCustomerRoomIdOut  { get; set; } 
}
public class CustomerGuestAppBuilderByCustomerRoomIdOut
{
    public int Id { get; set; }
    public string? Message { get; set; }
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
    public bool? IsActive { get; set; }
    public List<ModuleServiceOut> ModuleServiceOut { get; set; } = new List<ModuleServiceOut>();
}
public class ModuleServiceOut
{
    public string? name { get; set; }
    public int? displayOrder { get;set; }
    public bool? isDisable { get; set;}
    public string? image { get;set; }
    public int? items { get; set; }
    public int? categories { get; set; }
    public int? customerAppBuliderId { get; set; }
}
