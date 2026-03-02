using Dapper;
using MediatR;
using HospitioApi.Core.HandleUserAccount.Queries.GetDepartmentsUsers;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerUsers.Queries.GetDepartmentCustomerUsers;
public record GetDepartmentCustomerUsersRequest(GetDepartmentCustomerUsersIn In)
    : IRequest<AppHandlerResponse>;
public class GetDepartmentCustomerUsersHandler : IRequestHandler<GetDepartmentCustomerUsersRequest, AppHandlerResponse>
{
    //private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;
    public GetDepartmentCustomerUsersHandler(
        IDapperRepository dapper,
        IHandlerResponseFactory response)
    {
        // _db = db;
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetDepartmentCustomerUsersRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("SearchValue", request.In.SearchValue == null ? "" : request.In.SearchValue, DbType.String);
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);
        spParams.Add("SortColumn", request.In.SortColumn == null ? "RecentUser" : request.In.SortColumn, DbType.String);
        spParams.Add("SortOrder", request.In.SortOrder == null ? "DESC" : request.In.SortOrder, DbType.String);
        spParams.Add("CustomerId" , request.In.CustomerId, DbType.Int32);

        var customerdepartmentsUsersOut = await _dapper.GetAllJsonData<CustomerDepartmentsOut>("[dbo].[GetCustomerDepartmentUsers]"
        , spParams, cancellationToken,
        commandType: CommandType.StoredProcedure);
        if (customerdepartmentsUsersOut == null || customerdepartmentsUsersOut.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetDepartmentCustomerUsersOut("Get users successful.", customerdepartmentsUsersOut));
    }
}
