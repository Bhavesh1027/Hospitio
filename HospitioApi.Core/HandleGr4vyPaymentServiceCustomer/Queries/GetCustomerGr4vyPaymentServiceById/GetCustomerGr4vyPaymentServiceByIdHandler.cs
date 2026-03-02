using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Queries.GetCustomerGr4vyPaymentServiceById;
public record GetCustomerGr4vyPaymentServiceByIdHandlerRequest(GetCustomerGr4vyPaymentServiceByIdIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerGr4vyPaymentServiceByIdHandler : IRequestHandler<GetCustomerGr4vyPaymentServiceByIdHandlerRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerGr4vyPaymentServiceByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerGr4vyPaymentServiceByIdHandlerRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerPaymentProcessorId", request.In.CustomerPaymentProcessorId, DbType.Int32);

        var groupOut = await _dapper.GetAllJsonData<CustomerPaymentServiceByIdOut>("[dbo].[GetPaymentServiceByCustomerPaymentProcessorId]", spParams, cancellationToken, CommandType.StoredProcedure);

        if (groupOut == null || groupOut.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerGr4vyPaymentServiceByIdOut("Get customer payment service successful.", groupOut[0]));
    }
}
