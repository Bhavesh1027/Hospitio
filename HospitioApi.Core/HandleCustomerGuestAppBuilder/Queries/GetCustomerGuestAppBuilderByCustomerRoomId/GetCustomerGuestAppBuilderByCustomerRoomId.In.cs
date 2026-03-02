namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerGuestAppBuilderByCustomerRoomId;

public class GetCustomerGuestAppBuilderByCustomerRoomIdIn
{
    public int RoomId { get; set; } 
    public int? CustomerId { get; set; }
    public string? UserType { get; set; }
}
