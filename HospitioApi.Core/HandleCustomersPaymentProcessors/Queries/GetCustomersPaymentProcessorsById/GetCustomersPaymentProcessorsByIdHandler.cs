using Dapper;
using MediatR;
using HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessorsById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessorsById;
public record GetCustomersPaymentProcessorsByIdRequest(GetCustomersPaymentProcessorsByIdIn In) : IRequest<AppHandlerResponse>;
public class GetCustomersPaymentProcessorsByIdHandler : IRequestHandler<GetCustomersPaymentProcessorsByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomersPaymentProcessorsByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomersPaymentProcessorsByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, System.Data.DbType.Int32);

        var customersPaymentProcessors = await _dapper
            .GetAllJsonData<CustomersPaymentProcessorsByIdOut>("[dbo].[GetCustomersPaymentProcessorsById]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (customersPaymentProcessors == null || customersPaymentProcessors.Count <= 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomersPaymentProcessorsByIdOut("Get customers payment processors successful.", customersPaymentProcessors[0]));
    }
}
