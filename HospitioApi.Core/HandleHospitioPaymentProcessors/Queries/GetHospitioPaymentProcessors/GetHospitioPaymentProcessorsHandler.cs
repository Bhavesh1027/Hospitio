using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleHospitioPaymentProcessors.Queries.GetHospitioPaymentProcessors;
public record GetHospitioPaymentProcessorsRequest(GetHospitioPaymentProcessorsIn In) : IRequest<AppHandlerResponse>;
public class GetHospitioPaymentProcessorsHandler : IRequestHandler<GetHospitioPaymentProcessorsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetHospitioPaymentProcessorsHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetHospitioPaymentProcessorsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);
        // SP Name is GetHospitioPaymentProcessors
        var getHospitioPaymentProcessors = await _dapper.GetAll<HospitioPaymentProcessorsOut>("[dbo].[GetHospitioPaymentProcessors]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (getHospitioPaymentProcessors == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetHospitioPaymentProcessorsOut("Get hospitio payment processors successful.", getHospitioPaymentProcessors));

    }
}
