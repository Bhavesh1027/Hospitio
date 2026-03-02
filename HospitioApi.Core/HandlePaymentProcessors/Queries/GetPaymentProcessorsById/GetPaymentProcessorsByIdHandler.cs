using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandlePaymentProcessors.Queries.GetPaymentProcessorsById;
public record GetPaymentProcessorsByIdRequest(GetPaymentProcessorsByIdIn In) : IRequest<AppHandlerResponse>;
public class GetPaymentProcessorsByIdHandler : IRequestHandler<GetPaymentProcessorsByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetPaymentProcessorsByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetPaymentProcessorsByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var paymentProcessors = await _dapper.GetSingle<PaymentProcessorsByIdOut>("[dbo].[GetPaymentProcessorById]", spParams, cancellationToken, CommandType.StoredProcedure);

        if (paymentProcessors == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetPaymentProcessorsByIdOut("Get payment processor successful.", paymentProcessors));
    }
}
