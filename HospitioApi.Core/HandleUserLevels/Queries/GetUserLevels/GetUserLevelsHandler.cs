using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleUserLevels.Queries.GetUserLevels;
public record GetUserLevelsRequest(UserTypeEnum UserTypeEnum) : IRequest<AppHandlerResponse>;
public class GetUserLevelsHandler : IRequestHandler<GetUserLevelsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetUserLevelsHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetUserLevelsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        if (request.UserTypeEnum == UserTypeEnum.Hospitio)
        {
            spParams.Add("IsHospitioUserType", request.UserTypeEnum, DbType.Boolean);
        }
        // SP Name is GetGroups

        var userLevel = await _dapper.GetAll<UserLevelsOut>("[dbo].[GetUserLevels]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (userLevel == null || userLevel.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetUserLevelsOut("Get user levels successful.", userLevel));
    }
}
