using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleBusinessTypes.Queries.GetBusinessTypes;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleFeaturePermission.Queries.GetFeaturePermissions;
public record GetFeaturePermissionsRequest()
    : IRequest<AppHandlerResponse>;

public class GetFeaturePermissionsHandler : IRequestHandler<GetFeaturePermissionsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetFeaturePermissionsHandler(
        IDapperRepository dapper,
        IHandlerResponseFactory response
        )
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetFeaturePermissionsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();

        var permission = await _dapper
            .GetAll<GetFeaturePermissionsResponseOut>("[dbo].[GetPermissions]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (permission == null || permission.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetFeaturePermissionsOut("Get feature permission successful.", permission));
    }
}
