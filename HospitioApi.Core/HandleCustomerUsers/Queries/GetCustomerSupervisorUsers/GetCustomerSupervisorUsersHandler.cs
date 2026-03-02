using Dapper;
using MediatR;
using HospitioApi.Core.HandleUserAccount.Queries.GetUsers;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerSupervisorUsers;
public record GetCustomerSupervisorUsersRequest(GetCustomerSupervisorUsersIn In)
    : IRequest<AppHandlerResponse>;
public class GetCustomerSupervisorUsersHandler : IRequestHandler<GetCustomerSupervisorUsersRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;
    public GetCustomerSupervisorUsersHandler(
        ApplicationDbContext db, IDapperRepository dapper,
        IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
        _dapper = dapper;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerSupervisorUsersRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("DepartmentId", request.In.DepartmentId, DbType.Int32);
        spParams.Add("GroupId", request.In.GroupId, DbType.Int32);
        spParams.Add("CustomerUserLevelId", request.In.CustomerUserLevelId, DbType.Int32);
        spParams.Add("CustomerUserId", request.In.CustomerUserId, DbType.Int32);
        spParams.Add("CustomerId", request.In.CustomerId, DbType.Int32);

        var customerUsersOut = await _dapper.GetAllJsonData<CustomerUserOut>("[dbo].[GetCustomerUsers]"
        , spParams, cancellationToken,
        commandType: CommandType.StoredProcedure);

        if (customerUsersOut == null || customerUsersOut.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomerSupervisorUsersOut("Get users successful.", customerUsersOut));
    }
}
