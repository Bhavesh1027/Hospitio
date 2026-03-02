using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleTicket.Queries.GetTicketById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleTicket.Queries.GetTickets;

public record GetTicketsRequest(string UserId)
    : IRequest<AppHandlerResponse>;

public class GetTicketsHandler : IRequestHandler<GetTicketsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetTicketsHandler(
        IDapperRepository dapper,
        IHandlerResponseFactory response
        )
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetTicketsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.UserId, DbType.Int32);

        var tickets = await _dapper.GetAll<GetTicketsResponseOut>("[dbo].[GetTicketsByUserId]", spParams, cancellationToken, System.Data.CommandType.StoredProcedure);

        if (tickets == null || tickets.Count == 0)
        {
            return _response.Error("Tickets not found.", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetTicketsOut("Get tickets successful.", tickets));
    }
}
