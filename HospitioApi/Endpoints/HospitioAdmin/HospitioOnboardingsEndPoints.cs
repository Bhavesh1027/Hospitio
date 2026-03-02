using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleHospitioOnBoarding.Commands.UpdateHospitioOnBoardings;
using HospitioApi.Core.HandleHospitioOnBoarding.Queries.GetHospitioOnBoarding;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin
{
    public class HospitioOnboardingsEndPoints : IEndpointsModule
    {
        public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-admin/hospitioonboardings",
       singular: "api/hospitio-admin/hospitioonboarding");
        public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
        {
         app.MapGet($"/{Route.Singular}",GetHospitioOnboardingAsync)
        .AllowAnonymous(),
            app.MapPost($"/{Route.Singular}/update",UpdateHospitioOnboardingAsync)
        .AllowAnonymous(),
    };
        #region Delegate

        private async Task<IResult> GetHospitioOnboardingAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        {
            return await mtrHlpr.ToResultAsync(new GetHospitioOnBoardingRequest(), ct);
        }
        private async Task<IResult> UpdateHospitioOnboardingAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateHospitioOnBoardingsIn @in, CT ct)
            => await mtrHlpr.ToResultAsync(new UpdateHospitioOnBoardingsRequest(@in), ct);

        #endregion
    }
}
