using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleModules.Queries.GetModules;
public record GetModulesRequest() : IRequest<AppHandlerResponse>;
public class GetModulesHandler : IRequestHandler<GetModulesRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetModulesHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetModulesRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();

        var modules = await _dapper
            .GetAll<ModulesOut>("[dbo].[GetModules]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (modules == null || modules.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetModulesOut("Get modules successful.", modules));
    }
}
