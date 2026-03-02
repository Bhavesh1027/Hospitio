using FluentValidation;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomerById;

public class GetCustomerByIdValidator : AbstractValidator<GetCustomerByIdRequest>
{
    public GetCustomerByIdValidator()
    {

    }
}
