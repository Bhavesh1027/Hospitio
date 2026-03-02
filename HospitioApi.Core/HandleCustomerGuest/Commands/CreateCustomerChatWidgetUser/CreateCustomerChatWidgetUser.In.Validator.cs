using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.CreateCustomerChatWidgetUser;

public class CreateCustomerChatWidgetUserValidator : AbstractValidator<CreateCustomerChatWidgetUserRequest>
{
    public CreateCustomerChatWidgetUserValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateChatWidgetUserInValidator());
    }
    public class CreateChatWidgetUserInValidator : AbstractValidator<CreateCustomerChatWidgetUserIn>
    {
        public CreateChatWidgetUserInValidator()
        {
            RuleFor(m => m.EncryptedCustomerId).NotEmpty().NotNull();
        }
    }
}
