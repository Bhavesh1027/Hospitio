using FluentValidation;

namespace HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistants;

public class GetCustomersDigitalAssistantsValidator : AbstractValidator<GetCustomersDigitalAssistantsRequest>
{
    public GetCustomersDigitalAssistantsValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetDigitalAssistantsInValidator());

    }
    public class GetDigitalAssistantsInValidator : AbstractValidator<GetCustomersDigitalAssistantsIn>
    {
        public GetDigitalAssistantsInValidator()
        {
            //RuleFor(m => m.PageNo).NotEmpty().GreaterThan(0);
            //RuleFor(m => m.PageSize).NotEmpty().GreaterThan(0);
            RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
        }
    }
}
