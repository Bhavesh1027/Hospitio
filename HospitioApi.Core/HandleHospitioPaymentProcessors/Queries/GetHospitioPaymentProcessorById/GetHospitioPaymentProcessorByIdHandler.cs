using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleHospitioPaymentProcessors.Queries.GetHospitioPaymentProcessorById;
public record GetHospitioPaymentProcessorByIdRequest(GetHospitioPaymentProcessorByIdIn In) : IRequest<AppHandlerResponse>;
public class GetHospitioPaymentProcessorByIdHandler : IRequestHandler<GetHospitioPaymentProcessorByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetHospitioPaymentProcessorByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetHospitioPaymentProcessorByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var hospitioPaymentProcessorById = await _dapper.GetSingle<HospitioPaymentProcessorByIdOut>("[dbo].[GetHospitioPaymentProcessorById]", spParams, cancellationToken, CommandType.StoredProcedure);

        if (hospitioPaymentProcessorById == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetHospitioPaymentProcessorByIdOut("Get hospitio payment processor successful.", hospitioPaymentProcessorById));
    }
}
