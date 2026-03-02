using FluentValidation;

namespace HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Commands.UpdateGuestJourneyMessagesTemplates;

public class UpdateGuestJourneyMessagesTemplatesValidator : AbstractValidator<UpdateGuestJourneyMessagesTemplatesRequest>
{
    public UpdateGuestJourneyMessagesTemplatesValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateGuestJourneyMessagesTemplatesInValidator());
    }
    public class UpdateGuestJourneyMessagesTemplatesInValidator : AbstractValidator<UpdateGuestJourneyMessagesTemplatesIn>
    {
        public UpdateGuestJourneyMessagesTemplatesInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.TempleteType).NotEmpty();
            RuleFor(m => m.TempletMessage).NotEmpty();
        }
    }
}
