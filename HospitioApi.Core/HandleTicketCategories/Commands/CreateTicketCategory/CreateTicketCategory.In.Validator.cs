using FluentValidation;

namespace HospitioApi.Core.HandleTicketCategories.Commands.CreateTicketCategory;

public class CreateTicketCategoryValidator : AbstractValidator<CreateTicketCategoryRequest>
{
    public CreateTicketCategoryValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateTicketCategoryInValidator());
    }
    public class CreateTicketCategoryInValidator : AbstractValidator<CreateTicketCategoryIn>
    {
        public CreateTicketCategoryInValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
        }
    }
}
