using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using System.Data;

namespace HospitioApi.Core.HandleUserAccount.Queries.GetUserById;

public record GetUserByIdRequest(GetUserByIdIn In)
    : IRequest<AppHandlerResponse>;
public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public GetUserByIdHandler(IDapperRepository dapper,
        ApplicationDbContext db,
        IHandlerResponseFactory response)
    {
        _db = db;
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        UserByIdOut userOut = new();

        var spParams = new DynamicParameters();

        spParams.Add("Id", request.In.Id, DbType.Int32);


        var userByIdOut = await _dapper.GetAllJsonData<UserByIdOut>("[dbo].[GetUserById]"
        , spParams, cancellationToken,
        commandType: CommandType.StoredProcedure);

        return _response.Success(new GetUserByIdOut("Get user successful.", userByIdOut!.First()));
    }

}

