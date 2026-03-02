using FluentValidation;

namespace HospitioApi.Core.HandleCustomerHouseKeeping.Commands.UpdateCustomerHouseKeeping;

public class UpdateCustomerHouseKeepingValidator : AbstractValidator<UpdateCustomerHouseKeepingRequest>
{
    public UpdateCustomerHouseKeepingValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomerHouseKeepingInValidator());
    }

    public class UpdateCustomerHouseKeepingInValidator : AbstractValidator<UpdateCustomerHouseKeepingIn>
    {
        public UpdateCustomerHouseKeepingInValidator()
        {
            //RuleForEach(m => m.CustomerHouseKeepingCategories).ChildRules(category =>
            //{
            //    category.RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty();
            //    category.RuleFor(m => m.CategoryName).NotEmpty();
            //    category.RuleFor(m => m.DisplayOrder).NotEmpty();
            //    category.RuleForEach(m => m.CustomerHouseKeepingItems).ChildRules(items =>
            //    {
            //        items.RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty().GreaterThan(0);
            //        items.RuleFor(m => m.Name).MaximumLength(200);
            //        items.RuleFor(m => m.Currency).MaximumLength(3);
            //        items.RuleFor(m => m.DisplayOrder).NotEmpty();
            //    });
            //});
        }
    }
}
