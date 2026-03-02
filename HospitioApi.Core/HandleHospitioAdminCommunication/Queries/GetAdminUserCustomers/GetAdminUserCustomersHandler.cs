using Dapper;
using MediatR;
using HospitioApi.Shared;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleHospitioAdminCommunication.Queries.GetAdminUserCustomers;
public record GetAdminUserCustomersRequest(GetAdminUserCustomersIn In) : IRequest<AppHandlerResponse>;
public class GetAdminUserCustomersHandler : IRequestHandler<GetAdminUserCustomersRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetAdminUserCustomersHandler(IDapperRepository dapper,IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetAdminUserCustomersRequest request,CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("SearchString", request.In.SearchString, System.Data.DbType.String);
        spParams.Add("PageNo", request.In.PageNo, System.Data.DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, System.Data.DbType.Int32);
        spParams.Add("UserId", request.In.UserId, System.Data.DbType.Int32);

        var adminUserLists = await _dapper
            .GetAllJsonData<AdminUserCustomersOut>("[dbo].[SP_Get_AdminUserCustomersSearch]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if(adminUserLists == null || adminUserLists.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetAdminUserCustomersOut("Get admin user customers successfully.", adminUserLists));
    }
}
