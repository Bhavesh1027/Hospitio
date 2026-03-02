using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerAppBuilders;
public record GetCustomerGuestAppBuildersRequest(GetCustomerGuestAppBuildersIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerGuestAppBuildersHandler : IRequestHandler<GetCustomerGuestAppBuildersRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerGuestAppBuildersHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerGuestAppBuildersRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerId", request.In.CustomerId, DbType.Int32);
        spParams.Add("SearchColumn", request.In.SearchColumn, DbType.String);
        spParams.Add("SearchValue", request.In.SearchValue, DbType.String);
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);
        spParams.Add("SortColumn", request.In.SortColumn, DbType.String);
        spParams.Add("SortOrder", request.In.SortOrder, DbType.String);

        var result = await _dapper
            .GetAll<CustomerGuestAppBuildersOut>("[dbo].[GetCustomerGuestAppBuilders]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (result == null || result.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomerGuestAppBuildersOut("Get customer guest app builder successful.", result));
    }
}
