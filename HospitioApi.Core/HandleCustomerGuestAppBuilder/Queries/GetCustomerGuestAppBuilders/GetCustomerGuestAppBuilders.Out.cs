using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerAppBuilders;

public class GetCustomerGuestAppBuildersOut : BaseResponseOut
{
    public GetCustomerGuestAppBuildersOut(string message, List<CustomerGuestAppBuildersOut> customerGuestAppBuildersOut) : base(message)
    {
        CustomerGuestAppBuildersOut = customerGuestAppBuildersOut;
    }
    public List<CustomerGuestAppBuildersOut> CustomerGuestAppBuildersOut { get; set; }
}
public class CustomerGuestAppBuildersOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? CustomerRoomNameId { get; set; }
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
    public bool? IsActive { get; set; }
}
