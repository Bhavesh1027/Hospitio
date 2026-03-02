using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandlePaymentProcessors.Queries.GetPaymentProcessors;
public record GetPaymentProcessorsRequest() : IRequest<AppHandlerResponse>;
public class GetPaymentProcessorsHandler : IRequestHandler<GetPaymentProcessorsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetPaymentProcessorsHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetPaymentProcessorsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();


        var paymentProcessors = await _dapper.GetAll<PaymentProcessorsOut>("[dbo].[GetPaymentProcessors]", spParams, cancellationToken, CommandType.StoredProcedure);

        if (paymentProcessors == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetPaymentProcessorsOut("Get payment processor successful.", paymentProcessors));
    }
}
