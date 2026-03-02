using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;
using static HospitioApi.Core.HandleLeads.Queries.GetLeads.GetLeadsOut;

namespace HospitioApi.Core.HandleLeads.Queries.GetLeads;

public record GetLeadsRequest(GetLeadsIn In)
    : IRequest<AppHandlerResponse>;

public class GetLeadsHandler : IRequestHandler<GetLeadsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;
    public GetLeadsHandler(IDapperRepository dapper,
        ApplicationDbContext db,
        IHandlerResponseFactory response)
    {
        _db = db;
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetLeadsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("SearchValue", request.In.SearchValue, DbType.String);
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);
        spParams.Add("SortColumn", request.In.SortColumn, DbType.String);
        spParams.Add("SortOrder", request.In.SortOrder, DbType.String);
        spParams.Add("AlphabetsStartsWith", request.In.AlphabetsStartsWith, DbType.String);

        var leadsDapperOutsDemo = await _dapper.GetAll<LeadsOut>("[dbo].[GetLeads]"
                                    , spParams, cancellationToken,
        commandType: CommandType.StoredProcedure);

        if (leadsDapperOutsDemo == null || leadsDapperOutsDemo.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Forbidden403);
        }

        return _response.Success(new GetLeadsOut("Get leads successful.", leadsDapperOutsDemo!));
    }
}