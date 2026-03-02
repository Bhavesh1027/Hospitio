using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleVonageSMS.Commands.BuyCustomerVonageNumber;
using HospitioApi.Core.HandleVonageSMS.Commands.CancleCustomerVonageNumber;
using HospitioApi.Core.HandleVonageSMS.Commands.GetAvailableNumbers;
using HospitioApi.Core.HandleVonageSMS.Commands.UpdateCustomerVonageNumber;
using HospitioApi.Core.HandleVonageSMS.Queries.GetAvailableNumbers;
using HospitioApi.Core.HandleVonageSMS.Queries.GetCustomerOwnNumbers;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer
{
    public class VonageSMSEndPoint : IEndpointsModule
    {
        public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-customers/vonagenumber",
        singular: "api/hospitio-customer/vonagenumber");

        public RouteHandlerBuilder[] MapEndpoints(WebApplication app) =>
       new[]
       {
            app.MapGet($"/{Route.Singular}/search", GetAvailableNumbersAsync)
            .RequireAuthorization(),
            app.MapGet($"/{Route.Singular}/ownNumber", GetOwnNumbersAsync)
            .RequireAuthorization(),
              app.MapPost($"/{Route.Singular}/buyNumber", BuyNumberAsync)
            .RequireAuthorization(),
             app.MapPost($"/{Route.Singular}/updateNumber", UpdateNumberAsync)
            .RequireAuthorization(),
              app.MapPost($"/{Route.Singular}/cancleNumber", CancleNumberAsync)
            .RequireAuthorization(),
       };

        #region Delegates
        private async Task<IResult> GetAvailableNumbersAsync([FromServices] IMediatorHelper mtrHlpr, string? country, string type, string? pattern, int search_pattern, string? features, int? size, int? index, ClaimsPrincipal cp, CT ct)
        {
            GetAvailableNumbersIn @in = new()
            {
                CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId")),
                country= country,
                type= type,
                pattern= pattern,
                search_pattern= search_pattern,
                features= features,
                size= size,
                index= index
            };
            return await mtrHlpr.ToResultAsync(new GetAvailableNumbersHandlerRequest(@in), ct);
        }

        private async Task<IResult> GetOwnNumbersAsync([FromServices] IMediatorHelper mtrHlpr, string? pattern, int search_pattern,ClaimsPrincipal cp, CT ct)
        {
            GetCustomerOwnNumbersIn @in = new()
            {
                CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId")),
                pattern = pattern,
                search_pattern = search_pattern
            };
            return await mtrHlpr.ToResultAsync(new GetCustomerOwnNumbersHandlerRequest(@in), ct);
        }

        private async Task<IResult> BuyNumberAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] BuyCustomerVonageNumberIn @in , ClaimsPrincipal cp, CT ct)
        {

            @in.CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"));
        
            return await mtrHlpr.ToResultAsync(new BuyCustomerVonageNumberHandlerRequest(@in), ct);
        }

        private async Task<IResult> UpdateNumberAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] UpdateCustomerVonageNumberIn @in, ClaimsPrincipal cp, CT ct)
        {

            @in.CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"));

            return await mtrHlpr.ToResultAsync(new UpdateCustomerVonageNumberHandlerRequest(@in), ct);
        }

        private async Task<IResult> CancleNumberAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CancleCustomerVonageNumberIn @in, ClaimsPrincipal cp, CT ct)
        {

            @in.CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"));

            return await mtrHlpr.ToResultAsync(new CancleCustomerVonageNumberHandlerRequest(@in), ct);
        }
    };
}
#endregion

