using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Queries.GetCustomerEnhanceYourStayItemById;
public record GetCustomerEnhanceYourStayItemByIdRequest(GetCustomerEnhanceYourStayItemByIdIn In, string UserType) : IRequest<AppHandlerResponse>;
public class GetCustomerEnhanceYourStayItemByIdHandler : IRequestHandler<GetCustomerEnhanceYourStayItemByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerEnhanceYourStayItemByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerEnhanceYourStayItemByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);
        spParams.Add("UserType", request.UserType != null ? request.UserType : 3, DbType.Int32);

        var result = await _dapper.GetAllJsonData<CustomerEnhanceYourStayItemByIdOut>("[dbo].[GetCustomersEnhanceYourStayCategoryItemById]"
     , spParams, cancellationToken,
     commandType: CommandType.StoredProcedure);

        if (result == null || result.Count<=0)
        {
            return _response.Error("Customers enhance your stay item could not be found", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerEnhanceYourStayItemByIdOut("Get customer enhance your stay item successful.", result[0]));
    }
}
