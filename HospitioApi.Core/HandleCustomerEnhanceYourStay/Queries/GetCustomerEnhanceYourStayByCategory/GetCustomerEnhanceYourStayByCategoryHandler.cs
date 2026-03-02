using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStay.Queries.GetCustomerEnhanceYourStayByCategory;
public record GetCustomerEnhanceYourStayByCategoryRequest(GetCustomerEnhanceYourStayByCategoryIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerEnhanceYourStayByCategoryHandler : IRequestHandler<GetCustomerEnhanceYourStayByCategoryRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerEnhanceYourStayByCategoryHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerEnhanceYourStayByCategoryRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CategoryId", request.In.CategoryId, DbType.Int32);
        var getCustomersEnhanceYourStayCategoriesOuts = await _dapper.GetAllJsonData<CustomerEnhanceYourStayByCategoryOut>("[dbo].[GetCustomersEnhanceYourStayByCategory]"
      , spParams, cancellationToken,
      commandType: CommandType.StoredProcedure);
        if (getCustomersEnhanceYourStayCategoriesOuts == null || getCustomersEnhanceYourStayCategoriesOuts.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomerEnhanceYourStayByCategoryOut("Get customer enhance your stay categories successful.", getCustomersEnhanceYourStayCategoriesOuts));
    }
}
