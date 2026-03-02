using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomers.Commands.DeleteCustomer;

public class DeleteCustomerOut : BaseResponseOut
{
    public DeleteCustomerOut(string message , DeleteCustomerClass deleteCustomerClass) : base(message)
    {
        DeleteCustomerClass = deleteCustomerClass;
    }
    public DeleteCustomerClass DeleteCustomerClass { get; set; }
}

public class DeleteCustomerClass
{
    public int Id { get; set; }
}

