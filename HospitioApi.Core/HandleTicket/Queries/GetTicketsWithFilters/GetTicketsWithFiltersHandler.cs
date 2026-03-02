using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Migrations;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleTicket.Queries.GetTicketsWithFilters;

public record GetTicketsWithFiltersRequest(GetTicketsWithFiltersIn In, string UserId, UserTypeEnum UserType)
    : IRequest<AppHandlerResponse>;

public class GetTicketsWithFiltersHandler : IRequestHandler<GetTicketsWithFiltersRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetTicketsWithFiltersHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetTicketsWithFiltersRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        //spParams.Add("CategoryId", request.In.CategoryId ?? 0, DbType.Int32);
        spParams.Add("Status", request.In.Status ?? 0, DbType.Int16);
        spParams.Add("Priority", request.In.Priority ?? 0, DbType.Int16);
        spParams.Add("CustomerId", request.In.CustomerId ?? 0, DbType.Int32);
        spParams.Add("CSAgentId", request.In.CSAgentId ?? 0, DbType.Int32);
        spParams.Add("FromCreate", request.In.FromCreate, DbType.DateTime);
        spParams.Add("ToCreate", request.In.ToCreate, DbType.DateTime);
        spParams.Add("FromClose", request.In.FromClose, DbType.DateTime);
        spParams.Add("ToClose", request.In.ToClose, DbType.DateTime);
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);
        spParams.Add("ShortBy", request.In.ShortBy, DbType.Byte);
        spParams.Add("CreatedFrom", request.In.CreatedFrom, DbType.Byte);
        spParams.Add("ApplyPagination", request.In.ApplyPagination, DbType.Boolean);
        spParams.Add("UserId", request.UserId, DbType.Int32);
        spParams.Add("UserType", ((int)request.UserType).ToString());

        var result = await _dapper.GetAll<GetTicketsWithFiltersResponseOut>("[dbo].[GetTickets]", spParams, cancellationToken, System.Data.CommandType.StoredProcedure);

        if (result == null || result.Count == 0)
        {
            return _response.Error("Tickets not found.", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetTicketsWithFiltersOut("Get tickets successful.", result));

    }
}
