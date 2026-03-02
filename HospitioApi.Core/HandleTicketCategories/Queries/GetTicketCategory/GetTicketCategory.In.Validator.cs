using FluentValidation;

namespace HospitioApi.Core.HandleTicketCategories.Queries.GetTicketCategory;

public class GetTicketCategoryValidator : AbstractValidator<GetTicketCategoryRequest>
{
    public GetTicketCategoryValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetTicketCategoryInValidator());
    }
    public class GetTicketCategoryInValidator : AbstractValidator<GetTicketCategoryIn>
    {
        public GetTicketCategoryInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
