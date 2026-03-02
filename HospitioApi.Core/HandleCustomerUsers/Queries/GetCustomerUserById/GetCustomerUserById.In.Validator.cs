using FluentValidation;

namespace HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerUserById;

public class GetCustomerUserByIdValidator : AbstractValidator<GetCustomerUserByIdRequest>
{
    public GetCustomerUserByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerUserByValidator());
    }

    public class GetCustomerUserByValidator : AbstractValidator<GetCustomerUserByIdIn>
    {
        public GetCustomerUserByValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }

}
