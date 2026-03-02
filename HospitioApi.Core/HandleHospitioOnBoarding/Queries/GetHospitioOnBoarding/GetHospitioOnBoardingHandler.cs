using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleHospitioOnBoarding.Queries.GetHospitioOnBoarding;
public record GetHospitioOnBoardingRequest() : IRequest<AppHandlerResponse>;
public class GetHospitioOnBoardingHandler : IRequestHandler<GetHospitioOnBoardingRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;

    public GetHospitioOnBoardingHandler(IHandlerResponseFactory response, IDapperRepository dapper)
    {
        _response = response;
        _dapper = dapper;
    }

    public async Task<AppHandlerResponse> Handle(GetHospitioOnBoardingRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", 1, DbType.Int32);
        var hospitioOnBoarding = await _dapper.GetSingle<HospitioOnBoardingOut>("[dbo].[GetHospitioOnboarding]"
                                   , spParams, cancellationToken,
       commandType: CommandType.StoredProcedure);

        if (hospitioOnBoarding == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Forbidden403);
        }

        return _response.Success(new GetHospitioOnBoardingOut("Get hospitio on boarding successful.", hospitioOnBoarding));
    }
}
