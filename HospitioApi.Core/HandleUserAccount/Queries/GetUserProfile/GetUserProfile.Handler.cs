using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleUserAccount.Queries.GetUserProfile;
public record GetUserProfileRequest(GetUserProfileIn In) : IRequest<AppHandlerResponse>;

public class GetUserProfileHandler : IRequestHandler<GetUserProfileRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetUserProfileHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetUserProfileRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();

        spParams.Add("UserId", request.In.UserId, DbType.Int32);
        spParams.Add("UserType", ((UserTypeEnum)Convert.ToInt32(request.In.UserType)).ToString());


        var getUserProfileOut = await _dapper
            .GetSingle<GetProfileOut>("[dbo].[SP_GetUserProfile]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (getUserProfileOut == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetUserProfileOut("Get user profile successful.", getUserProfileOut));
    }

}

