using Dapper;
using MediatR;
using HospitioApi.Core.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfoByAppBuilderId;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleDisplayorder.Queries.GetDisplayOrder;
public record GetDisplayOrderRequest(GetDisplayOrderIn In) : IRequest<AppHandlerResponse>;
public class GetDisplayOrderHandler : IRequestHandler<GetDisplayOrderRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetDisplayOrderHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetDisplayOrderRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("ReferenceId", request.In.ReferenceId, DbType.Int32);
        spParams.Add("ScreenName", ScreenDisplayOrder.PropertyInfo, DbType.Int32);

        var getDiasplayOrder = await _dapper.GetSingle<DisplayOrderOut>("[dbo].[GetDisplayOrder]", spParams, cancellationToken, CommandType.StoredProcedure);

        if (getDiasplayOrder == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetDisplayOrderOut("Get successful.", getDiasplayOrder));
    }
}
