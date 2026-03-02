using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleTicketCategories.Commands.DeleteTicketCategory;
public record DeleteTicketCategoryRequest(DeleteTicketCategoryIn In) : IRequest<AppHandlerResponse>;
public class DeleteTicketCategoryHandler : IRequestHandler<DeleteTicketCategoryRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public DeleteTicketCategoryHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(DeleteTicketCategoryRequest request, CancellationToken cancellationToken)
    {
        var ticketcategory = await _db.TicketCategorys.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (ticketcategory == null)
        {
            return _response.Error($"Ticket category with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }
        _db.TicketCategorys.Remove(ticketcategory);
        await _db.SaveChangesAsync(cancellationToken);
        return _response.Success(new DeleteTicketCategoryOut("Delete ticket category successful.", new() { Id = ticketcategory.Id }));
    }

}
