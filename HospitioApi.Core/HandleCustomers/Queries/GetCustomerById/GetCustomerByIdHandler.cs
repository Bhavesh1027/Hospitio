using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomerById;
public record GetCustomerByIdRequest(GetCustomerByIdIn In, string CustomerId) : IRequest<AppHandlerResponse>;
public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.CustomerId, DbType.Int32);

        var customer = await _dapper.GetAllJsonData<CustomerByIdOut>("[dbo].[GetCustomer]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (customer == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomerByIdOut("Get customer successful.", customer));
    }
}
