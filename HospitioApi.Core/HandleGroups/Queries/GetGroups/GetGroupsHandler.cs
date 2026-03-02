using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleGroups.Queries.GetGroups;
public record GetGroupsRequest(GetGroupsIn In) : IRequest<AppHandlerResponse>;
public class GetGroupsHandler : IRequestHandler<GetGroupsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetGroupsHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetGroupsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("UserType", request.In.UserType, DbType.Int32);
        spParams.Add("UserId", request.In.UserId, DbType.Int32);

        // SP Name is GetGroups

        var group = await _dapper.GetAll<GroupsOut>("[dbo].[GetGroups]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (group == null || group.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetGroupsOut("Get groups successful.", group));


    }

}
