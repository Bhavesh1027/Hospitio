using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerRoomNames.Queries.GetCustomerRoomNames;

public class GetCustomerRoomNamesOut : BaseResponseOut
{
    public GetCustomerRoomNamesOut(string message, List<CustomerAppBuilders> customerRoomNamesOuts) : base(message)
    {
        CustomerRoomNamesOut = customerRoomNamesOuts;
    }

    public List<CustomerAppBuilders> CustomerRoomNamesOut { get; set; } = new List<CustomerAppBuilders>();
}

public class CustomerAppBuilders
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public byte? IsWork { get; set; }
    public string? BizType { get; set; }
    public int? NoOfRooms { get; set; }
}