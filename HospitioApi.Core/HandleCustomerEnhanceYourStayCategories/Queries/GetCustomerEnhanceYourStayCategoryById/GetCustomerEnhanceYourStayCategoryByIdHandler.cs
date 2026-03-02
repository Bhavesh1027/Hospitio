using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Queries.GetCustomerEnhanceYourStayCategoryById;
public record GetCustomerEnhanceYourStayCategoryByIdRequest(GetCustomerEnhanceYourStayCategoryByIdIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerEnhanceYourStayCategoryWithRelationHandler : IRequestHandler<GetCustomerEnhanceYourStayCategoryByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerEnhanceYourStayCategoryWithRelationHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerEnhanceYourStayCategoryByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var result = await _dapper.GetSingle<CustomerEnhanceYourStayCategoryByIdOut>("[dbo].[GetCustomersEnhanceYourStayCategoryById]"
     , spParams, cancellationToken,
     commandType: CommandType.StoredProcedure);

        if (result == null)
        {
            return _response.Error("Customers enhance your stay category could not be found", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerEnhanceYourStayCategoryByIdOut("Get customer enhance your stay category successful.", result));
    }
}
