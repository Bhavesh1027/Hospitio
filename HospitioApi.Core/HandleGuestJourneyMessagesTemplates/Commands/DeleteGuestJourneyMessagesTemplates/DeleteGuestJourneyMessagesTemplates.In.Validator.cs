using FluentValidation;

namespace HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Commands.DeleteGuestJourneyMessagesTemplates;

public class DeleteGuestJourneyMessagesTemplatesValidator : AbstractValidator<DeleteGuestJourneyMessagesTemplatesRequest>
{
    public DeleteGuestJourneyMessagesTemplatesValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteGuestJourneyMessagesTemplatesInValidator());
    }
    public class DeleteGuestJourneyMessagesTemplatesInValidator : AbstractValidator<DeleteGuestJourneyMessagesTemplatesIn>
    {
        public DeleteGuestJourneyMessagesTemplatesInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
