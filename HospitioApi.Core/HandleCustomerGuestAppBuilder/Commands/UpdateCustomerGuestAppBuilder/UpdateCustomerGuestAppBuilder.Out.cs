using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Commands.UpdateCustomerAppBuilder;

public class UpdateCustomerGuestAppBuilderOut : BaseResponseOut
{
    public UpdateCustomerGuestAppBuilderOut(string message, UpdatedCustomerGuestAppBuilderOut updatedCustomerGuestAppBuilderOut) : base(message)
    {
        UpdatedCustomerGuestAppBuilderOut = updatedCustomerGuestAppBuilderOut;
    }
    public UpdatedCustomerGuestAppBuilderOut UpdatedCustomerGuestAppBuilderOut { get; set; }
}
public class UpdatedCustomerGuestAppBuilderOut
{
    public int Id { get; set; }
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
    public bool? OnlineCheckIn { get; set; }
    public bool? IsActive { get; set; }
}
