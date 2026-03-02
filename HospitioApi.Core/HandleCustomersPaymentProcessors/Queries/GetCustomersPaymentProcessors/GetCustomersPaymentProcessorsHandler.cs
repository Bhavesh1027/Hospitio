using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessors;
public record GetCustomersPaymentProcessorsRequest(GetCustomersPaymentProcessorsIn In) : IRequest<AppHandlerResponse>;
public class GetCustomersPaymentProcessorsHandler : IRequestHandler<GetCustomersPaymentProcessorsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomersPaymentProcessorsHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomersPaymentProcessorsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);
        spParams.Add("CustomerId", request.In.CustomerId, System.Data.DbType.Int32);
        var customersPaymentProcessors = await _dapper
            .GetAll<CustomersPaymentProcessorsOut>("[dbo].[GetCustomerPaymentProcessors]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (customersPaymentProcessors == null || customersPaymentProcessors.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomersPaymentProcessorsOut("Get customer payment processors successful.", customersPaymentProcessors));
    }

}
