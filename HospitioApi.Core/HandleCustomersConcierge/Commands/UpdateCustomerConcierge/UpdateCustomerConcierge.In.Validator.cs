using FluentValidation;

namespace HospitioApi.Core.HandleCustomersConcierge.Commands.UpdateCustomerConcierge;

public class UpdateCustomerConciergeValidator : AbstractValidator<UpdateCustomerConciergeRequest>
{
    public UpdateCustomerConciergeValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomerConciergeInValidator());
    }

    public class UpdateCustomerConciergeInValidator : AbstractValidator<UpdateCustomerConciergeIn>
    {
        public UpdateCustomerConciergeInValidator()
        {
            //RuleForEach(m => m.CustomerConciergeCategories).ChildRules(category =>
            //{
            //    category.RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty();
            //    category.RuleFor(m => m.CategoryName).NotEmpty();
            //    category.RuleFor(m => m.DisplayOrder).NotEmpty();
            //    category.RuleForEach(m => m.CustomerConciergeItems).ChildRules(items =>
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
