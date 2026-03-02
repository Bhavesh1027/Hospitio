using FluentValidation;

namespace HospitioApi.Core.HandleTicketCategories.Commands.DeleteTicketCategory;

public class DeleteTicketCategoryValidator : AbstractValidator<DeleteTicketCategoryRequest>
{
    public DeleteTicketCategoryValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteTicketCategoryInValidator());
    }
    public class DeleteTicketCategoryInValidator : AbstractValidator<DeleteTicketCategoryIn>
    {
        public DeleteTicketCategoryInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
