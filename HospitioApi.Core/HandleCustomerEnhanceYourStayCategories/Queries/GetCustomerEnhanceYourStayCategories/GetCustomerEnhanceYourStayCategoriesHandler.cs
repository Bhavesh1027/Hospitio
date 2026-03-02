using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using System.Data;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategories;
public record GetCustomerEnhanceYourStayCategoriesRequest(GetCustomerEnhanceYourStayCategoriesIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerEnhanceYourStayCategoriesHandler : IRequestHandler<GetCustomerEnhanceYourStayCategoriesRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerEnhanceYourStayCategoriesHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerEnhanceYourStayCategoriesRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("SearchValue", request.In.SearchValue == null ? "" : request.In.SearchValue, DbType.String);
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);
        spParams.Add("SortColumn", request.In.SortColumn == null ? "" : request.In.SortColumn, DbType.String);
        spParams.Add("SortOrder", request.In.SortOrder == null ? "" : request.In.SortOrder, DbType.String);
        spParams.Add("CustomerId", request.In.CustomerId, DbType.Int32);
        var getCustomersEnhanceYourStayCategoriesOuts = await _dapper.GetAll<CustomerEnhanceYourStayCategoriesOut>("[dbo].[GetCustomersEnhanceYourStayCategories]"
      , spParams, cancellationToken,
      commandType: CommandType.StoredProcedure);

        return _response.Success(new GetCustomerEnhanceYourStayCategoriesOut("Get customer enhance your stay categories successful.", getCustomersEnhanceYourStayCategoriesOuts));
    }
}

