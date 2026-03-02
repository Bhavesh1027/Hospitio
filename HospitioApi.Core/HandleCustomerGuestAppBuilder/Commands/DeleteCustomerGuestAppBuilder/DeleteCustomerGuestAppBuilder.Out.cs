using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Commands.DeleteCustomerAppBuilder;

public class DeleteCustomerGuestAppBuilderOut : BaseResponseOut
{
    public DeleteCustomerGuestAppBuilderOut(string message, DeletedCustomerGuestAppBuilderOut deletedCustomerGuestAppBuilderOut) : base(message)
    {

        DeletedCustomerGuestAppBuilderOut = deletedCustomerGuestAppBuilderOut;
    }
    public DeletedCustomerGuestAppBuilderOut DeletedCustomerGuestAppBuilderOut { get; set; }
}
public class DeletedCustomerGuestAppBuilderOut
{
    public int Id { get; set; }
}
