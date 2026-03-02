using Dapper;
using MediatR;
using HospitioApi.Core.HandleUserAccount.Queries.GetUserById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerUsers.Queries.GetCustomerUserById;
public record GetCustomerUserByIdRequest(GetCustomerUserByIdIn In)
    : IRequest<AppHandlerResponse>;
public class GetCustomerUserByIdHandler : IRequestHandler<GetCustomerUserByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerUserByIdHandler(IDapperRepository dapper,
        ApplicationDbContext db,
        IHandlerResponseFactory response)
    {
        _db = db;
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerUserByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var customerUserByIdOut = await _dapper.GetAllJsonData<CustomerUserByIdOut>("[dbo].[GetCustomerUserById]"
        , spParams, cancellationToken,
        commandType: CommandType.StoredProcedure);

        if (customerUserByIdOut == null || customerUserByIdOut.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerUserByIdOut("Get user successful.", customerUserByIdOut!.First()));
    }

}
