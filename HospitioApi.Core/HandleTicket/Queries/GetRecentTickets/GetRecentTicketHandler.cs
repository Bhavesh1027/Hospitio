using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleTicket.Queries.GetRecentTickets;
public record GetRecentTicketsRequest(GetRecentTicketIn In)
    : IRequest<AppHandlerResponse>;
public class GetRecentTicketHandler : IRequestHandler<GetRecentTicketsRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;

    public GetRecentTicketHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response,
        IDapperRepository dapper
        )
    {
        _response = response;
        _dapper = dapper;
    }
    public async Task<AppHandlerResponse> Handle(GetRecentTicketsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerId", request.In.CustomerId, DbType.Int32);
        var getRecentTicketsResponseOuts = await _dapper.GetAll<GetRecentTicketsResponseOut>("[dbo].[GetRecentTicketByCustomerId]"
      , spParams, cancellationToken,
      commandType: CommandType.StoredProcedure);
        if (getRecentTicketsResponseOuts.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetRecentTicketOut("Get recent tickets successful.", getRecentTicketsResponseOuts));
    }
}
