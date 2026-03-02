using FluentValidation;

namespace HospitioApi.Core.HandleCustomersConcierge.Commands.CreateCustomerConcierge;

public class CreateCustomerConciergeValidator : AbstractValidator<CreateCustomerConciergeRequest>
{
    public CreateCustomerConciergeValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerConciergeInValidator());
    }
    public class CreateCustomerConciergeInValidator : AbstractValidator<CreateCustomerConciergeIn>
    {
        public CreateCustomerConciergeInValidator()
        {
            RuleForEach(m => m.CustomerConciergeCategories).ChildRules(category =>
            {
                category.RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
                category.RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty();
                category.RuleFor(m => m.CategoryName).NotEmpty();
                category.RuleFor(m => m.DisplayOrder).NotEmpty();
                category.RuleForEach(m => m.CustomerConciergeItems).ChildRules(items =>
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
