using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuest.Queries.GetCustomerGuests;

public class GetCustomerGuestsValidator : AbstractValidator<GetCustomerGuestsRequest>
{
    public GetCustomerGuestsValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerGuestsInValidator());

    }
    public class GetCustomerGuestsInValidator : AbstractValidator<GetCustomerGuestsIn>
    {
        public GetCustomerGuestsInValidator()
        {
            RuleFor(m => m.PageNo).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageSize).NotEmpty().GreaterThan(0);
        }
    }
}
