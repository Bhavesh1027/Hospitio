using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleUserAccount.Queries.GetDepartmentsUsers;
public record GetDepartmentsUsersRequest(GetDepartmentsUsersIn In)
    : IRequest<AppHandlerResponse>;

public class GetDepartmentsUsersHandler : IRequestHandler<GetDepartmentsUsersRequest, AppHandlerResponse>
{
    //private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;
    public GetDepartmentsUsersHandler(
        IDapperRepository dapper,
        IHandlerResponseFactory response)
    {
        // _db = db;
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetDepartmentsUsersRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("SearchValue", request.In.SearchValue == null ? "" : request.In.SearchValue, DbType.String);
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);
        spParams.Add("SortColumn", request.In.SortColumn == null ? "RecentUser" : request.In.SortColumn, DbType.String);
        spParams.Add("SortOrder", request.In.SortOrder == null ? "DESC" : request.In.SortOrder, DbType.String);

        var departmentsUsersOut = await _dapper.GetAllJsonData<DepartmentsOut>("[dbo].[GetDepartmentsUsers]"
        , spParams, cancellationToken,
        commandType: CommandType.StoredProcedure);
        if (departmentsUsersOut == null || departmentsUsersOut.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetDepartmentsUsersOut("Get users successful.", departmentsUsersOut));
    }
}