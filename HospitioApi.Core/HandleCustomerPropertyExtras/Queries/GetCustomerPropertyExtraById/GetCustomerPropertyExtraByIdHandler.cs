using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerPropertyExtras.Queries.GetCustomerPropertyExtraById;
public record GetCustomerPropertyExtraByIdRequest(GetCustomerPropertyExtraByIdIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerPropertyExtraByIdHandler : IRequestHandler<GetCustomerPropertyExtraByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerPropertyExtraByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerPropertyExtraByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var result = await _dapper.GetSingle<CustomerPropertyExtraByIdOut>("[dbo].[GetCustomerPropertyExtraById]"
     , spParams, cancellationToken,
     commandType: CommandType.StoredProcedure);

        if (result == null)
        {
            return _response.Error("Customers digital assistant could not be found", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerPropertyExtraByIdOut("Get customers digital assistant successful.", result));
    }
}
