using FluentValidation;

namespace HospitioApi.Core.HandleCustomerReception.Commands.UpdateCustomerReception;

public class UpdateCustomerReceptionValidator : AbstractValidator<UpdateCustomerReceptionRequest>
{
    public UpdateCustomerReceptionValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomerReceptionInValidator());
    }

    public class UpdateCustomerReceptionInValidator : AbstractValidator<UpdateCustomerReceptionIn>
    {
        public UpdateCustomerReceptionInValidator()
        {
            //RuleForEach(m => m.CustomerReceptionCategories).ChildRules(category =>
            //{
            //    category.RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty();
            //    category.RuleFor(m => m.CategoryName).NotEmpty();
            //    category.RuleFor(m => m.DisplayOrder).NotEmpty();
            //    category.RuleForEach(m => m.CustomerReceptionItems).ChildRules(items =>
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
