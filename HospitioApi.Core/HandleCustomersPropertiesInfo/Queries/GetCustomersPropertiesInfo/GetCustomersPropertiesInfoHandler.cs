using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfo;
public record GetCustomersPropertiesInfoRequest(GetCustomersPropertiesInfoIn In) : IRequest<AppHandlerResponse>;
public class GetCustomersPropertiesInfoHandler : IRequestHandler<GetCustomersPropertiesInfoRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomersPropertiesInfoHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomersPropertiesInfoRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("SearchColumn", request.In.SearchColumn == null ? "" : request.In.SearchColumn, DbType.String);
        spParams.Add("SearchValue", request.In.SearchValue == null ? "" : request.In.SearchValue, DbType.String);
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);
        spParams.Add("SortColumn", request.In.SortColumn == null ? "" : request.In.SortColumn, DbType.String);
        spParams.Add("SortOrder", request.In.SortOrder == null ? "" : request.In.SortOrder, DbType.String);
        spParams.Add("CustomerId", request.In.CustomerId, DbType.Int32);
        var getCustomersPropertiesInfoOuts = await _dapper.GetAll<CustomersPropertiesInfoOut>("[dbo].[GetCustomersPropertiesInfo]"
      , spParams, cancellationToken,
      commandType: CommandType.StoredProcedure);
        if (getCustomersPropertiesInfoOuts== null || getCustomersPropertiesInfoOuts.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomersPropertiesInfoOut("Get property info successful.", getCustomersPropertiesInfoOuts));
    }
}

