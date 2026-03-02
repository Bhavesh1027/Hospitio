using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleTicketCategories.Queries.GetTicketCategory;
public record GetTicketCategoryRequest(GetTicketCategoryIn In) : IRequest<AppHandlerResponse>;
public class GetTicketCategoryHandler : IRequestHandler<GetTicketCategoryRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public GetTicketCategoryHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetTicketCategoryRequest request, CancellationToken cancellationToken)
    {
        var result = await _db.TicketCategorys.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (result == null)
        {
            return _response.Error($"Ticket category with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }
        var ticketCategoryOut = new TicketCategoryOut()
        {
            Id = result.Id,
            Name = result.CategoryName!
        };
        return _response.Success(new GetTicketCategoryOut("Get ticket category successful.", ticketCategoryOut));
    }
}
