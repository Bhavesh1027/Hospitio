using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleGroups.Queries.GetGroupsByDepartmentId;
public record GetGroupsByDepartmentIdRequest(GetGroupsByDepartmentIdIn In) : IRequest<AppHandlerResponse>;
public class GetGroupsByDepartmentIdHandler: IRequestHandler<GetGroupsByDepartmentIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetGroupsByDepartmentIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetGroupsByDepartmentIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("DepartmentId", request.In.DepartmentId, DbType.Int32);
        spParams.Add("UserType" , request.In.UserType, DbType.Int32);

        var group = await _dapper.GetAll<GroupsByDepartmentIdOut>("[dbo].[GetGroupsByDepartmentId]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (group == null || group.Count == 0)
        {
            return _response.Error("No group available.", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetGroupsByDepartmentIdOut("Get groups successful.", group));


    }
}
