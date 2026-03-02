using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerAppBuilderById;
public record GetCustomerGuestAppBuilderByIdRequest(GetCustomerGuestAppBuilderByIdIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerGuestAppBuilderByIdHandler : IRequestHandler<GetCustomerGuestAppBuilderByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerGuestAppBuilderByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerGuestAppBuilderByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var customerGuestAppBuilderByIdOut = await _dapper
            .GetSingle<CustomerGuestAppBuilderByIdOut>("[dbo].[GetCustomerGuestAppBuilderById]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (customerGuestAppBuilderByIdOut == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerGuestAppBuilderByIdOut("Get customer guest app builder successful.", customerGuestAppBuilderByIdOut));
    }
}
