using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.CreateCustomerChatWidgetUser;

public class CreateCustomerChatWidgetUserOut : BaseResponseOut
{
    public CreateCustomerChatWidgetUserOut(string message, CreatedCustomerChatWidgetUserOut createdCustomerGuestOut) : base(message)
    {
        CreatedCustomerGuestOut = createdCustomerGuestOut;
    }
    public CreatedCustomerChatWidgetUserOut CreatedCustomerGuestOut { get; set; }
}
public class CreatedCustomerChatWidgetUserOut
{
    public string Link { get; set; } = string.Empty;
    public int CustomerUserId { get; set; }
    public int WidgetUserId { get; set; }
    public string? Cname { get; set; }
    public string? BusinessName { get; set; }

}
