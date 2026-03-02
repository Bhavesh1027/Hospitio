using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleTicket.Queries.GetTicketById;

public record GetTicketByIdRequest(int Id)
    : IRequest<AppHandlerResponse>;

public class GetTicketByIdHandler : IRequestHandler<GetTicketByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    //private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public GetTicketByIdHandler(
        IDapperRepository dapper,
        IHandlerResponseFactory response
        )
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetTicketByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.Id, DbType.Int32);

        var ticket = await _dapper.GetAllJsonData<GetTicketByIdResponseOut>("[dbo].[GetTicketById]", spParams, cancellationToken, System.Data.CommandType.StoredProcedure);

        if (ticket == null || ticket.Count == 0)
        {
            return _response.Error("Tickets not found.", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetTicketByIdOut("Get ticket successful.", ticket));
    }
}
