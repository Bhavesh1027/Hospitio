using FluentValidation;

namespace HospitioApi.Core.HandleQaCategories.Queries.GetQaCategory;

public class GetQaCategoryValidator : AbstractValidator<GetQaCategoryRequest>
{
    public GetQaCategoryValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetQaCategoryInValidator());
    }
    public class GetQaCategoryInValidator : AbstractValidator<GetQaCategoryIn>
    {
        public GetQaCategoryInValidator()
        {
            RuleFor(m => m.Id).GreaterThanOrEqualTo(1);
        }
    }
}

