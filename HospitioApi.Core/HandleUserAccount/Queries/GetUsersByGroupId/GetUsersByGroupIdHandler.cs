using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleUserAccount.Queries.GetUsersByGroupId;
public record GetUsersByGroupIdRequest(GetUsersByGroupIdIn In)
    : IRequest<AppHandlerResponse>;
public class GetUsersByGroupIdHandler : IRequestHandler<GetUsersByGroupIdRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;
    public GetUsersByGroupIdHandler(
        IDapperRepository dapper,
        IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetUsersByGroupIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("GroupId", request.In.GroupId, DbType.Int32);

        var departmentsUsersOut = await _dapper.GetAll<UsersByGroupIdOut>("[dbo].[GetUsersByGroupId]"
        , spParams, cancellationToken,
        commandType: CommandType.StoredProcedure);
        if (departmentsUsersOut == null || departmentsUsersOut.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetUsersByGroupIdOut("Get users successful.", departmentsUsersOut));
    }
}
    