using FluentValidation;

namespace HospitioApi.Core.HandleCustomerHouseKeeping.Commands.CreateCustomerHouseKeeping;

public class CreateCustomerHouseKeepingValidator : AbstractValidator<CreateCustomerHouseKeepingRequest>
{
    public CreateCustomerHouseKeepingValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerHouseKeepingInValidator());
    }
    public class CreateCustomerHouseKeepingInValidator : AbstractValidator<CreateCustomerHouseKeepingIn>
    {
        public CreateCustomerHouseKeepingInValidator()
        {
            RuleForEach(m => m.CustomerHouseKeepingCategories).ChildRules(category =>
            {
                category.RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
                category.RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty();
                category.RuleFor(m => m.CategoryName).NotEmpty();
                category.RuleFor(m => m.DisplayOrder).NotEmpty();
                category.RuleForEach(m => m.CustomerHouseKeepingItems).ChildRules(items =>
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
