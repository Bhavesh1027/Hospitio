using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleUserAccount.Queries.GetUsers;

public record GetUsersRequest(GetUsersIn In)
    : IRequest<AppHandlerResponse>;

public class GetUsersHandler : IRequestHandler<GetUsersRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;
    public GetUsersHandler(
        ApplicationDbContext db, IDapperRepository dapper,
        IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
        _dapper = dapper;
    }

    public async Task<AppHandlerResponse> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("DepartmentId", request.In.DepartmentId, DbType.Int32);
        spParams.Add("GroupId", request.In.GroupId, DbType.Int32);
        spParams.Add("UserLevelId", request.In.UserLevelId, DbType.Int32);
        spParams.Add("UserId", request.In.UserId, DbType.Int32);

        var usersOut = await _dapper.GetAllJsonData<UserOut>("[dbo].[GetUsers]"
        , spParams, cancellationToken,
        commandType: CommandType.StoredProcedure);

        if (usersOut == null || usersOut.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetUsersOut("Get users successful.", usersOut));
    }
}

