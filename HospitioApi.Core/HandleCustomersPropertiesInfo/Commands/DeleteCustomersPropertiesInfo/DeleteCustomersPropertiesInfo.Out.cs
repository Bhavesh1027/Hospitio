using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersPropertiesInfo.Commands.DeleteCustomersPropertiesInfo;

public class DeleteCustomersPropertiesInfoOut : BaseResponseOut
{
    public DeleteCustomersPropertiesInfoOut(string message, DeletedCustomersPropertiesInfoOut deletedCustomersPropertiesInfoOut) : base(message)
    {
        DeletedCustomersPropertiesInfoOut = deletedCustomersPropertiesInfoOut;
    }
    public DeletedCustomersPropertiesInfoOut DeletedCustomersPropertiesInfoOut { get; set; }
}
public class DeletedCustomersPropertiesInfoOut
{
    public int Id { get; set; }
}