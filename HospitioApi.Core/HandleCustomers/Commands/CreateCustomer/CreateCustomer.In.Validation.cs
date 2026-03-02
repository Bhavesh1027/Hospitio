using FluentValidation;

namespace HospitioApi.Core.HandleCustomers.Commands.CreateCustomer;

public class CreateCustomerValidation : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerValidation()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerInValidation());
    }

    public class CreateCustomerInValidation : AbstractValidator<CreateCustomerIn>
    {
        public CreateCustomerInValidation()
        {
            RuleFor(m => m.BusinessName).NotEmpty();
            RuleFor(m => m.NoOfRooms).NotEmpty();
            RuleFor(m => m.BusinessTypeId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.CustomerUserIn).NotEmpty();
            RuleFor(m => m.PhoneCountry).NotEmpty().MaximumLength(3);
            RuleFor(m => m.WatsappCountry).MaximumLength(3);
            RuleFor(m => m.ViberCountry).MaximumLength(3);
            RuleFor(m => m.TelegramCounty).MaximumLength(3);
            //RuleFor(m => m.CustomerUserIn.Title).NotEmpty().MaximumLength(3);
            RuleFor(m => m.ProductId).NotEmpty().GreaterThan(0);
        }
    }
}
