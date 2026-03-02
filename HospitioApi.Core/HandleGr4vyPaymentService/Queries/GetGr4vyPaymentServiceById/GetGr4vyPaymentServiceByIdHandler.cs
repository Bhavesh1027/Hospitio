using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleGr4vyPaymentService.Queries.GetGr4vyPaymentServiceById;
public record GetGr4vyPaymentServiceByIdHandlerRequest(GetGr4vyPaymentServiceByIdIn In) : IRequest<AppHandlerResponse>;
public class GetGr4vyPaymentServiceByIdHandler : IRequestHandler<GetGr4vyPaymentServiceByIdHandlerRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetGr4vyPaymentServiceByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetGr4vyPaymentServiceByIdHandlerRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("HospitioPaymentProcessorId", request.In.HospitioPaymentProcessorId, DbType.Int32);

        var groupOut = await _dapper.GetAllJsonData<PaymentServiceByIdOut>("[dbo].[GetPaymentServiceByPaymentHospitioProcessorId]", spParams, cancellationToken, CommandType.StoredProcedure);

        if (groupOut == null || groupOut.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetGr4vyPaymentServiceByIdOut("Get payment service successful.", groupOut[0]));
    }
}
