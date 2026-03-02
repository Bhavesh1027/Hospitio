using HospitioApi.Shared;
namespace HospitioApi.Core.HandleCustomerPropertyExtras.Commands.CreateCustomerPropertyExtras;

public class CreateCustomerPropertyExtrasOut : BaseResponseOut
{
    public CreateCustomerPropertyExtrasOut(string message, CreatedCustomerPropertyExtrasOut createdCustomerPropertyExtrasOut) : base(message)
    {
        CreatedCustomerPropertyExtrasOut = createdCustomerPropertyExtrasOut;
    }
    public CreatedCustomerPropertyExtrasOut CreatedCustomerPropertyExtrasOut { get; set; }
}
public class CreatedCustomerPropertyExtrasOut
{
    public int Id { get; set; }

}