using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStay.Queries.GetCustomerEnhanceYourStay;
public record GetCustomerEnhanceYourStayRequest(GetCustomerEnhanceYourStayIn In, string UserType) : IRequest<AppHandlerResponse>;
public class GetCustomerEnhanceYourStayHandler: IRequestHandler<GetCustomerEnhanceYourStayRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerEnhanceYourStayHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerEnhanceYourStayRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("BuilderId", request.In.BuilderId, DbType.Int32);
        spParams.Add("UserType", request.UserType != null ? request.UserType : 3, DbType.Int32);
        var getCustomersEnhanceYourStayCategoriesOuts = await _dapper.GetAllJsonData<CustomerEnhanceYourStayOut>("[dbo].[GetCustomersEnhanceYourStay]"
      , spParams, cancellationToken,
      commandType: CommandType.StoredProcedure);
        if (getCustomersEnhanceYourStayCategoriesOuts == null || getCustomersEnhanceYourStayCategoriesOuts.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomerEnhanceYourStayOut("Get customer enhance your stay categories successful.", getCustomersEnhanceYourStayCategoriesOuts));
    }
}
