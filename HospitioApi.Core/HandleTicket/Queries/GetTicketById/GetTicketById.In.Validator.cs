using FluentValidation;
namespace HospitioApi.Core.HandleTicket.Queries.GetTicketById;
public class GetTicketByIdValidator : AbstractValidator<GetTicketByIdRequest>
{
    public GetTicketByIdValidator()
    {
        RuleFor(m => m.Id).GreaterThan(0);
    }
}
