using FluentValidation;

namespace HospitioApi.Core.HandleTicketCategories.Commands.UpdateTicketCategory;

public class UpdateTicketCategoryValidator : AbstractValidator<UpdateTicketCategoryRequest>
{
    public UpdateTicketCategoryValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateTicketCategoryInValidator());
    }
    public class UpdateTicketCategoryInValidator : AbstractValidator<UpdateTicketCategoryIn>
    {
        public UpdateTicketCategoryInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            RuleFor(m => m.CategoryName).NotEmpty();
        }
    }
}
