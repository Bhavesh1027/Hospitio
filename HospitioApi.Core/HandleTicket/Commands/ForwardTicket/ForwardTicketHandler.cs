using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleTicket.Commands.ForwardTicket;
public record ForwardTicketRequest(ForwardTicketIn In) : IRequest<AppHandlerResponse>;
public class ForwardTicketHandler : IRequestHandler<ForwardTicketRequest, AppHandlerResponse>
{
	private readonly ApplicationDbContext _db;
	private readonly IHandlerResponseFactory _response;

	public ForwardTicketHandler(ApplicationDbContext db, IHandlerResponseFactory response)
	{
		_db = db;
		_response = response;
	}
	public async Task<AppHandlerResponse> Handle(ForwardTicketRequest request, CancellationToken cancellationToken)
	{
		var ticket = await _db.Tickets.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);

		if (ticket == null)
		{
			return _response.Error($"Ticket with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
		}

		ticket.CSAgentId = request.In.UserId > 0 ? request.In.UserId : null;
		ticket.GroupId = request.In.GroupId;
		ticket.Status = (byte?)TicketStatus.Assigned;
		await _db.SaveChangesAsync(cancellationToken);

		return _response.Success(new ForwardTicketOut("Ticket forward successful."));


	}
}
