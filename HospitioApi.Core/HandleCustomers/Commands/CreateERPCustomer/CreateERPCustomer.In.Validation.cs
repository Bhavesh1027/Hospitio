using FluentValidation;

namespace HospitioApi.Core.HandleCustomers.Commands.CreateERPCustomer;

public class CreateERPCustomerValidaton : AbstractValidator<CreateERPCustomerRequest>
{
    public CreateERPCustomerValidaton()
    {
        RuleFor(m => m.In).SetValidator(new CreateERPCustomerInValidation());
    }

    public class CreateERPCustomerInValidation : AbstractValidator<CreateERPCustomerIn>
    {
        public CreateERPCustomerInValidation()
        {
            RuleFor(m => m.PylonUniqueCustomerId).NotNull().NotEmpty();
            RuleFor(m => m.FirstName).NotNull().NotEmpty();
            RuleFor(m => m.LastName).NotNull().NotEmpty();
            RuleFor(m => m.CompanyName).NotNull().NotEmpty();
            RuleFor(m => m.BusinessType).NotNull().NotEmpty();
            RuleFor(m => m.NoOfRooms).NotNull().NotEmpty();
            RuleFor(m => m.ServicePack).NotNull().NotEmpty();
            RuleFor(m => m.ExpirationInDay).NotNull();
            RuleFor(m => m.Email).NotNull().NotEmpty();
            RuleFor(m => m.Username).NotNull().NotEmpty();
            RuleFor(m => m.Password).NotNull().NotEmpty();
        }
    }
}
