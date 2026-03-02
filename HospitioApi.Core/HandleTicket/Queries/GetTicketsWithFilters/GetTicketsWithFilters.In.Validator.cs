using FluentValidation;
using HospitioApi.Core.HandleTicket.Queries.GetTicketsWithFilters;

namespace HospitioApi.Core.HandleTicket.Queries.GetTicketsWithFiltersWithFilters;
public class GetTicketsWithFiltersValidator : AbstractValidator<GetTicketsWithFiltersRequest>
{
    public GetTicketsWithFiltersValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetTicketsWithFiltersInValidator());
    }

    public class GetTicketsWithFiltersInValidator : AbstractValidator<GetTicketsWithFiltersIn>
    {
        public GetTicketsWithFiltersInValidator()
        {
            RuleFor(m => m.PageNo).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageSize).NotEmpty().GreaterThan(0);
        }
    }
}
