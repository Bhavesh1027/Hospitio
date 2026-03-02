using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleModuleServices.Queries.GetModuleServiceById;
public record GetModuleServiceByIdRequest(GetModuleServiceByIdIn In) : IRequest<AppHandlerResponse>;
public class GetModuleServiceByIdHandler : IRequestHandler<GetModuleServiceByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetModuleServiceByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetModuleServiceByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var moduleServiceById = await _dapper.GetSingle<ModuleServiceById>("[dbo].[GetModuleServicesById]", spParams, cancellationToken, CommandType.StoredProcedure);

        if (moduleServiceById == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetModuleServiceByIdOut("Get module service successful.", moduleServiceById));

    }
}
