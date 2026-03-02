using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Commands.CreateCustomerAppBuilder;

public class CreateCustomerGuestAppBuilderOut : BaseResponseOut
{
    public CreateCustomerGuestAppBuilderOut(string message, CreatedCustomerGuestAppBuilderOut createdCustomerGuestAppBuilderOut) : base(message)
    {
        CreatedCustomerGuestAppBuilderOut = createdCustomerGuestAppBuilderOut;
    }
    public CreatedCustomerGuestAppBuilderOut CreatedCustomerGuestAppBuilderOut { get; set; }
}
public class CreatedCustomerGuestAppBuilderOut
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


