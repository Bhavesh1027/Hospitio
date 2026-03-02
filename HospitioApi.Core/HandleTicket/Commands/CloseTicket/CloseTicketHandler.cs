using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleTicket.Commands.CloseTicket;
public record CloseTicketRequest(CloseTicketIn In) : IRequest<AppHandlerResponse>;
public class CloseTicketHandler : IRequestHandler<CloseTicketRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public CloseTicketHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(CloseTicketRequest request, CancellationToken cancellationToken)
    {
        var ticket = await _db.Tickets.Where(e => e.Id == request.In.TicketId).FirstOrDefaultAsync(cancellationToken);

        if (ticket == null)
        {
            return _response.Error($"Ticket with Id {request.In.TicketId} could not be found.", AppStatusCodeError.Gone410);
        }

        ticket.Status = (byte?)TicketStatus.Closed;
        ticket.CloseDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new CloseTicketOut("Ticket close successful."));
    }
}
