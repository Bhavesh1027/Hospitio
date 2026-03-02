using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleTicket.Commands.UpdateTicketPriority;
public record UpdateTicketPriorityRequest(UpdateTicketPriorityIn In) : IRequest<AppHandlerResponse>;
public class UpdateTicketPriorityHandler : IRequestHandler<UpdateTicketPriorityRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public UpdateTicketPriorityHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdateTicketPriorityRequest request, CancellationToken cancellationToken)
    {
        var ticketStatus = await _db.Tickets.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (ticketStatus == null)
        {
            return _response.Error($"Given ticket not exists.", AppStatusCodeError.UnprocessableEntity422);
        }

        ticketStatus.Priority = request.In.Priority;

        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new UpdateTicketPriorityOut("Update ticket status successful.", new()
        {
            Id = request.In.Id,
            Priority = request.In.Priority,
        }));
    }
}