using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerGuest.Queries.GetCustomerGuestById;
public record GetCustomerGuestByIdRequest(GetCustomerGuestByIdIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerGuestByIdHandler : IRequestHandler<GetCustomerGuestByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerGuestByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerGuestByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var customerGuestByIdOut = await _dapper
            .GetSingle<CustomerGuestByIdOut>("[dbo].[GetCustomerGuestById]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (customerGuestByIdOut == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerGuestByIdOut("Get customer guest successful.", customerGuestByIdOut));
    }
}
