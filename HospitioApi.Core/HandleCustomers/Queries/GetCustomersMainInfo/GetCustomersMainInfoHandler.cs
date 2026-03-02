using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomersMainInfo;
public record GetCustomersMainInfoRequest(GetCustomersMainInfoIn In) : IRequest<AppHandlerResponse>;
public class GetCustomersMainInfoHandler : IRequestHandler<GetCustomersMainInfoRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomersMainInfoHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomersMainInfoRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("SearchValue", request.In.SearchValue, DbType.String);
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);
        spParams.Add("SortColumn", request.In.SortColumn, DbType.String);
        spParams.Add("SortOrder", request.In.SortOrder, DbType.String);
        spParams.Add("AlphabetsStartsWith", request.In.AlphabetsStartsWith, DbType.String); 

        var customersMainInfo = await _dapper
            .GetAll<CustomersMainInfoOut>("[dbo].[GetCustomers]", spParams, cancellationToken, CommandType.StoredProcedure);

        if (customersMainInfo == null || customersMainInfo.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomersMainInfoOut("Get customer main info successful.", customersMainInfo));
    }
}
