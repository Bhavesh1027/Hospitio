using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Queries.GetCustomerGr4vyPaymentService;
public record GetCustomerGr4vyPaymentServiceRequest(GetCustomerGr4vyPaymentServiceIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerGr4vyPaymentServiceHandler : IRequestHandler<GetCustomerGr4vyPaymentServiceRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerGr4vyPaymentServiceHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerGr4vyPaymentServiceRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerId", request.In.CustomerId, System.Data.DbType.Int32);

        var paymentServicesOuts = await _dapper
            .GetAll<CustomerPaymentServicesOut>("[dbo].[GetCustomerPaymentServices]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (paymentServicesOuts == null || paymentServicesOuts.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerGr4vyPaymentServiceOut("Get customer payment service successful.", paymentServicesOuts));
    }
}
