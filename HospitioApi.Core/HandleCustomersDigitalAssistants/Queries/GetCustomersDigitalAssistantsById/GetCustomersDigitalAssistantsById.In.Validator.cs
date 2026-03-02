using FluentValidation;

namespace HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistantsById;

public class GetCustomersDigitalAssistantsByIdValidator : AbstractValidator<GetCustomersDigitalAssistantsByIdRequest>
{
    public GetCustomersDigitalAssistantsByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomersDigitalAssistantsByIdInValidator());
    }
    public class GetCustomersDigitalAssistantsByIdInValidator : AbstractValidator<GetCustomersDigitalAssistantsByIdIn>
    {
        public GetCustomersDigitalAssistantsByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
