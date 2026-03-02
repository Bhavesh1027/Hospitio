using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerMerchantAccount.Queries.GetCustomerMerchantAccount;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CreateMerchantAccountEndpoints
{
    //public AppRoute Route { get; init; } = new(
    //        plural__: "api/hospitio-customer/merchantaccounts",
    //        singular: "api/hospitio-customer/merchantaccount");
    //public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    //{
    //        app.MapGet($"/{Route.Singular}", GetmerchantaccountbycustomerIdAsync)
    //        .RequireAuthorization()
    //    };
    //#region Delegates
    //private async Task<IResult> GetmerchantaccountbycustomerIdAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    //{
    //    GetCustomerMerchantAccountIn @in = new()
    //    {
    //        CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"))
    //    };
    //    return await mtrHlpr.ToResultAsync(new GetCustomerMerchantAccountRequest(@in), ct);
    //}

    //#endregion
}
