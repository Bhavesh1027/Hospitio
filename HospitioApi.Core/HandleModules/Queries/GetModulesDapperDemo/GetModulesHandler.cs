using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using System.Data;

namespace HospitioApi.Core.HandleModules.Queries.GetModulesDapperDemo;

public record GetModulesDapperDemoRequest(GetModulesDapperDemoIn In) : IRequest<AppHandlerResponse>;
public class GetModulesDapperDemoHandler : IRequestHandler<GetModulesDapperDemoRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetModulesDapperDemoHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetModulesDapperDemoRequest request, CancellationToken cancellationToken)
    {
        var modulesDapperOutsDemo = await _dapper.GetAllJsonData<Module>("[dbo].[GetModuleJson_sample]"
      , null, cancellationToken,
      commandType: CommandType.StoredProcedure);
        //Nested Objec SP JSON DATA END


        var modulesDapperOuts = new List<ModulesDapperOut>();

        return _response.Success(new GetModulesDapperOut("Get modules successful.", modulesDapperOuts));
    }
}
