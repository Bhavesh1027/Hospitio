using FluentValidation;

namespace HospitioApi.Core.HandleCustomersGuestJourneys.Commands.CreateCustomersGuestJourneys;

public class CreateCustomersGuestJourneysValidator : AbstractValidator<CreateCustomersGuestJourneysRequest>
{
    public CreateCustomersGuestJourneysValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomersGuestJourneysInValidator());
    }
    public class CreateCustomersGuestJourneysInValidator : AbstractValidator<CreateCustomersGuestJourneysIn>
    {
        public CreateCustomersGuestJourneysInValidator()
        {
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
