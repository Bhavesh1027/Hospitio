using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Queries.GetCustomerGuestAppEnhanceYourStayItemsExtraByStayId;
public record GetCustomerGuestAppEnhannceYourStayItemExtraRequest(GetCustomerGuestAppEnhannceYourStayItemExtraIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerGuestAppEnhannceYourStayItemExtraHandler : IRequestHandler<GetCustomerGuestAppEnhannceYourStayItemExtraRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerGuestAppEnhannceYourStayItemExtraHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerGuestAppEnhannceYourStayItemExtraRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerGuestAppEnhanceYourStayItemId", request.In.CustomerGuestAppEnhanceYourStayItemId, DbType.Int32);
        var customersPropertiesInfoOuts = await _dapper.GetAll<CustomersGuestAppEnhanceYourStayCategoryItemsExtraOut>("[dbo].[GetCustomerGuestAppEnhanceYourStayItemsExtraByStayId]"
      , spParams, cancellationToken,
      commandType: CommandType.StoredProcedure);
        if (customersPropertiesInfoOuts.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomerGuestAppEnhannceYourStayItemExtraOut("Get guest app enhance your stay catefory items successful.", customersPropertiesInfoOuts));
    }
}
