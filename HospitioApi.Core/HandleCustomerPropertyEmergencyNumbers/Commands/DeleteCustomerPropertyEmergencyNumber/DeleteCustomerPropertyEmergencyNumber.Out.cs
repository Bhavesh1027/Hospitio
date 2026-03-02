using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.DeleteCustomerPropertyEmergencyNumber;

public class DeleteCustomerPropertyEmergencyNumberOut : BaseResponseOut
{
    public DeleteCustomerPropertyEmergencyNumberOut(string message, DeletedCustomerPropertyEmergencyNumberOut deletedCustomerPropertyEmergencyNumberOut) : base(message)
    {
        DeletedCustomerPropertyEmergencyNumberOut = deletedCustomerPropertyEmergencyNumberOut;
    }
    public DeletedCustomerPropertyEmergencyNumberOut DeletedCustomerPropertyEmergencyNumberOut { get; set; }
}
public class DeletedCustomerPropertyEmergencyNumberOut
{
    public int Id { get; set; }
}
