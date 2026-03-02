using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleMusement.Queries.GetMusementData;
public record GetMusementDataRequest(GetMusementDataIn In) : IRequest<AppHandlerResponse>;
public class GetMusementDataHandler : IRequestHandler<GetMusementDataRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetMusementDataHandler(
        IDapperRepository dapper,
        IHandlerResponseFactory response
        )
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetMusementDataRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("SearchString", request.In.SearchValue, DbType.String);
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);

        var musementData = await _dapper.GetAllJsonData<GetMusementDataResponseOut>("[dbo].[GetMusementData]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (musementData == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetMusementDataOut("Get MusementData successful.", musementData));
    }
}
