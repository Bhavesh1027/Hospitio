using FluentValidation;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomers.Commands.UpdateCustomer;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerValidator()
    {
        //RuleFor(m => m.In).SetValidator(new UpdateCustomerInValidator());
        When(m => m.UserType != UserTypeEnum.Customer, () =>
        {
            RuleFor(m => m.In).SetValidator(new UpdateCustomerInValidator());
        }).Otherwise(() =>
        {
            RuleFor(customer => customer.In).SetValidator(new UpdateCustomerInValidatorForHospitioAdmin());
        });
    }
    public class UpdateCustomerInValidatorForHospitioAdmin : AbstractValidator<UpdateCustomerIn>
    {
        public UpdateCustomerInValidatorForHospitioAdmin()
        {
            RuleFor(m => m.BusinessName).NotEmpty();
            RuleFor(m => m.NoOfRooms).NotEmpty();
            RuleFor(m => m.BusinessTypeId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.IncomingTranslationLangage).NotEmpty().MaximumLength(10);

        }
    }
    public class UpdateCustomerInValidator : AbstractValidator<UpdateCustomerIn>
    {
        public UpdateCustomerInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            RuleFor(m => m.BusinessName).NotEmpty();
            RuleFor(m => m.NoOfRooms).NotEmpty();
            RuleFor(m => m.BusinessTypeId).NotEmpty().GreaterThan(0);
            RuleFor(m => m.IncomingTranslationLangage).NotEmpty().MaximumLength(10);
            RuleFor(m => m.CurrencyCode).NotEmpty().MaximumLength(3);
        }
    }
}
