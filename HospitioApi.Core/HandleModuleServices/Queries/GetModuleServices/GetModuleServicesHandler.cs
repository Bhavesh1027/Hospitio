using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleModuleServices.Queries.GetModuleServices;
public record GetModuleServicesRequest() : IRequest<AppHandlerResponse>;
public class GetModuleServicesHandler : IRequestHandler<GetModuleServicesRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetModuleServicesHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetModuleServicesRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();

        var moduleServices = await _dapper
            .GetAll<ModuleServicesOut>("[dbo].[GetModuleServices]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (moduleServices == null || moduleServices.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetModuleServicesOut("Get module services successful.", moduleServices));
    }
}
