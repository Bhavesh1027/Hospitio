using FluentValidation;

namespace HospitioApi.Core.HandleCustomerReception.Commands.CreateCustomerReception;

public class CreateCustomerReceptionValidator : AbstractValidator<CreateCustomerReceptionRequest>
{
    public CreateCustomerReceptionValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerReceptionInValidator());
    }
    public class CreateCustomerReceptionInValidator : AbstractValidator<CreateCustomerReceptionIn>
    {
        public CreateCustomerReceptionInValidator()
        {
            RuleForEach(m => m.CuastomerReceptionCategories).ChildRules(category =>
            {
                category.RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
                category.RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty();
                category.RuleFor(m => m.CategoryName).NotEmpty();
                category.RuleFor(m => m.DisplayOrder).NotEmpty();
                category.RuleForEach(m => m.CustomerReceptionItems).ChildRules(items =>
                {
                    items.RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
                    items.RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty().GreaterThan(0);
                    items.RuleFor(m => m.Name).MaximumLength(200);
                    items.RuleFor(m => m.Currency).MaximumLength(3);
                    items.RuleFor(m => m.DisplayOrder).NotEmpty();
                });
            });
        }
    }
}
