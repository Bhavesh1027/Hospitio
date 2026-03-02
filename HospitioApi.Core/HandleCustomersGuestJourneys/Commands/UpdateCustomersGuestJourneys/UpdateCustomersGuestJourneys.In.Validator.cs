using FluentValidation;

namespace HospitioApi.Core.HandleCustomersGuestJourneys.Commands.UpdateCustomersGuestJourneys;

public class UpdateCustomersGuestJourneysValidator : AbstractValidator<UpdateCustomersGuestJourneysRequest>
{
    public UpdateCustomersGuestJourneysValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomersGuestJourneysInValidator());
    }

    public class UpdateCustomersGuestJourneysInValidator : AbstractValidator<UpdateCustomersGuestJourneysIn>
    {
        public UpdateCustomersGuestJourneysInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            RuleFor(m => m.JourneyStep).NotEmpty();
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.SendType).NotEmpty();
            RuleFor(m => m.TimingOption1).NotEmpty();
            RuleFor(m => m.TimingOption2).NotEmpty();
            RuleFor(m => m.TimingOption3).NotEmpty();
            RuleFor(m => m.Timing).NotEmpty();
            //RuleFor(m => m.NotificationDays).NotEmpty();
            //RuleFor(m => m.NotificationTime).NotEmpty();
            //RuleFor(m => m.GuestJourneyMessagesTemplateId).NotEmpty();
            RuleFor(m => m.TempletMessage).NotEmpty();
        }
    }
}
