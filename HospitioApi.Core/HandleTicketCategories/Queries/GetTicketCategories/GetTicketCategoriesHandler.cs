using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTicketCategories.Queries.GetTicketCategories;
public record GetTicketCategoriesRequest() : IRequest<AppHandlerResponse>;
public class GetTicketCategoriesHandler : IRequestHandler<GetTicketCategoriesRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public GetTicketCategoriesHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetTicketCategoriesRequest request, CancellationToken cancellationToken)
    {
        var ticketCategories = await _db.TicketCategorys.Select(e => new TicketCategoriesOut()
        {
            Id = e.Id,
            Name = e.CategoryName!
        }).ToListAsync(cancellationToken);
        return _response.Success(new GetTicketCategoriesOut("Get ticket categories successful.", ticketCategories));
    }
}
