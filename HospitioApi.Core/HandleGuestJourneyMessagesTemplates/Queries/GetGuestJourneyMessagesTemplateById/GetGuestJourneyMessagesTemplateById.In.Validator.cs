using FluentValidation;

namespace HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Queries.GetGuestJourneyMessagesTemplateById;

public class GetGuestJourneyMessagesTemplateByIdValidator : AbstractValidator<GetGuestJourneyMessagesTemplateByIdReuest>
{
    public GetGuestJourneyMessagesTemplateByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetGuestJourneyMessagesTemplateByIdInValidator());
    }
    public class GetGuestJourneyMessagesTemplateByIdInValidator : AbstractValidator<GetGuestJourneyMessagesTemplateByIdIn>
    {
        public GetGuestJourneyMessagesTemplateByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
