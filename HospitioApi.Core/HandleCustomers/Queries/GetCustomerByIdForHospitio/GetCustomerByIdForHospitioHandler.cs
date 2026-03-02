using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomerByIdForHospitio;
public record GetCustomerByIdForHospitioRequest(GetCustomerByIdForHospitioIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerByIdForHospitioHandler : IRequestHandler<GetCustomerByIdForHospitioRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerByIdForHospitioHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerByIdForHospitioRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var customer = await _dapper.GetSingle<CustomerByIdForHospitioOut>("[dbo].[GetCustomerForHospitioAdmin]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (customer == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerByIdForHospitioOut("Get customer successful.", customer));
    }
}
