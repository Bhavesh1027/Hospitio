using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.SendWelcomeMessage;

public class SendWelcomeMessageValidator : AbstractValidator<SendWelcomeMessageRequest>
{
    public SendWelcomeMessageValidator()
    {
        RuleFor(m => m.In).SetValidator(new SendWelcomeMessageInValidator());
    }
    public class SendWelcomeMessageInValidator : AbstractValidator<SendWelcomeMessageIn>
    {
        public SendWelcomeMessageInValidator()
        {
            RuleFor(m => m.GuestId).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
