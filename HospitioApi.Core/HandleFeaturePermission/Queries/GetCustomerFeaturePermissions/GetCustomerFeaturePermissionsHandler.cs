using Dapper;
using MediatR;
using HospitioApi.Core.HandleFeaturePermission.Queries.GetFeaturePermissions;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleFeaturePermission.Queries.GetCustomerFeaturePermissions;

public record GetCustomerFeaturePermissionsRequest()
    : IRequest<AppHandlerResponse>;
public class GetCustomerFeaturePermissionsHandler : IRequestHandler<GetCustomerFeaturePermissionsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerFeaturePermissionsHandler(
        IDapperRepository dapper,
        IHandlerResponseFactory response
        )
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerFeaturePermissionsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();

        var permission = await _dapper
            .GetAll<GetCustomerFeaturePermissionsResponseOut>("[dbo].[GetCustomerPermissions]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (permission == null || permission.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerFeaturePermissionsOut("Get feature permission successful.", permission));
    }
}
