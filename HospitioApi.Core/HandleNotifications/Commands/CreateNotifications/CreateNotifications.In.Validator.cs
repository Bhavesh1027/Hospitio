using FluentValidation;

namespace HospitioApi.Core.HandleNotifications.Commands.CreateNotifications;

public class CreateNotificationsValidator : AbstractValidator<CreateNotificationsRequest>
{
    public CreateNotificationsValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateNotificationsInValidator());
    }

    public class CreateNotificationsInValidator : AbstractValidator<CreateNotificationsIn>
    {
        public CreateNotificationsInValidator()
        {
          //  RuleFor(m => m.Country).NotEmpty();
          //  RuleFor(m => m.City).NotEmpty();
            RuleFor(m => m.Title).NotEmpty();
            RuleFor(m => m.Message).NotEmpty();
          //  RuleFor(m => m.Postalcode).NotEmpty();
           // RuleFor(m => m.BusinessTypeId).NotEmpty().GreaterThan(0);
          //  RuleFor(m => m.ProductId).NotEmpty().GreaterThan(0);
        }
    }
}
