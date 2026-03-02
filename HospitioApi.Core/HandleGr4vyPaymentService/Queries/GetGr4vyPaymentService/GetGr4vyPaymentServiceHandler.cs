using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleGr4vyPaymentService.Queries.GetGr4vyPaymentService;
public record GetGr4vyPaymentServiceRequest() : IRequest<AppHandlerResponse>;
public class GetGr4vyPaymentServiceHandler : IRequestHandler<GetGr4vyPaymentServiceRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetGr4vyPaymentServiceHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetGr4vyPaymentServiceRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();

        var paymentServicesOuts = await _dapper
            .GetAll<HospitioPaymentServicesOut>("[dbo].[GetHospitioPaymentServices]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (paymentServicesOuts == null || paymentServicesOuts.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetGr4vyPaymentServiceOut("Get hospitio payment service successful.", paymentServicesOuts));
    }
}
