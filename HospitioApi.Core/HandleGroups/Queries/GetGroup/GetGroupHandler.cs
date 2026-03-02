using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleGroups.Queries.GetGroup;
public record GetGroupHandlerRequest(GetGroupIn In) : IRequest<AppHandlerResponse>;
public class GetGroupHandler : IRequestHandler<GetGroupHandlerRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetGroupHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetGroupHandlerRequest request, CancellationToken cancellationToken)
    {

        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);
        spParams.Add("UserType" , request.In.UserType, DbType.Int32);

        var groupOut = await _dapper.GetSingle<GroupOut>("[dbo].[GetGroupById]", spParams, cancellationToken, CommandType.StoredProcedure);

        if (groupOut == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetGroupOut("Get group successful.", groupOut));
    }
}
