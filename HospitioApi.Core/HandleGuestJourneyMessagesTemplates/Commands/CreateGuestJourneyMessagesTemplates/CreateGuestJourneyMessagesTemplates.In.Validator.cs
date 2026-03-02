using FluentValidation;

namespace HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Commands.CreateGuestJourneyMessagesTemplates;

public class CreateGuestJourneyMessagesTemplatesValidator : AbstractValidator<CreateGuestJourneyMessagesTemplatesRequest>
{
    public CreateGuestJourneyMessagesTemplatesValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateGuestJourneyMessagesTemplatesInValidator());
    }
    public class CreateGuestJourneyMessagesTemplatesInValidator : AbstractValidator<CreateGuestJourneyMessagesTemplatesIn>
    {
        public CreateGuestJourneyMessagesTemplatesInValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.TempleteType).NotEmpty();
            RuleFor(m => m.TempletMessage).NotEmpty();
        }
    }
}
