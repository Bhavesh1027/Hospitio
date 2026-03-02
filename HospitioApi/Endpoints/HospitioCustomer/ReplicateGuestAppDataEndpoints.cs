using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleAccount.Commands.CustomerLogin;
using HospitioApi.Core.HandleCustomers.Commands.CreateCustomer;
using HospitioApi.Core.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfoByAppBuilderId;
using HospitioApi.Core.HandleReplicateDateForGuestApp.Commands.ReplicateGuestData;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

    public class ReplicateGuestAppDataEndpoints: IEndpointsModule
    {

        public AppRoute Route { get; init; } = new(singular: "api/hospitio-customer/replicateguestdata");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[] {
         app.MapPost($"/{Route.Singular}", ReplicateDataAsync)
            .AllowAnonymous(),
    };

    #region Delegates

    private async Task<IResult> ReplicateDataAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] ReplicateDataIn @in, ClaimsPrincipal cp, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new ReplicateDataRequest(@in), ct);
    }
      
    #endregion
}
