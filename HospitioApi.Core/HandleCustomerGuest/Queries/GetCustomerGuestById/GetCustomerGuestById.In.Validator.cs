using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuest.Queries.GetCustomerGuestById;

public class GetCustomerGuestByIdValidator : AbstractValidator<GetCustomerGuestByIdRequest>
{
    public GetCustomerGuestByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerGuestByIdInValidator());

    }
    public class GetCustomerGuestByIdInValidator : AbstractValidator<GetCustomerGuestByIdIn>
    {
        public GetCustomerGuestByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
