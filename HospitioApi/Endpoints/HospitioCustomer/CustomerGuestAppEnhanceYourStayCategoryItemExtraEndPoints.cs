using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Commands.CreateCustomerGuestAppEnhanceYourStayCategoryItemExtra;
using HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Commands.DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtra;
using HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Queries.GetCustomerGuestAppEnhanceYourStayItemsExtraByStayId;
using HospitioApi.Helpers;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerGuestAppEnhanceYourStayCategoryItemExtraEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-customer/guest-app-enhance-your-stay-category-items-extra",
       singular: "api/hospitio-customer/guest-app-enhance-your-stay-category-item-extra");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetCustomerGuestAppEnhanceYourStayCategoryItemsExtraAsync)
        .RequireAuthorization(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/create", CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraAsync)
        .RequireAuthorization()
        };

    #region Delegates
    private async Task<IResult> GetCustomerGuestAppEnhanceYourStayCategoryItemsExtraAsync([FromServices] IMediatorHelper mtrHlpr, CT ct, int CustomerGuestAppEnhanceYourStayItemId)
    {
        GetCustomerGuestAppEnhannceYourStayItemExtraIn @in = new()
        {
            CustomerGuestAppEnhanceYourStayItemId = CustomerGuestAppEnhanceYourStayItemId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestAppEnhannceYourStayItemExtraRequest(@in), ct);

    }

    private async Task<IResult> DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int CustomerGuestAppEnhanceYourStayItemId, CT ct)
    {
        DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraIn @in = new() { CustomerGuestAppEnhanceYourStayItemId = CustomerGuestAppEnhanceYourStayItemId };
        return await mtrHlpr.ToResultAsync(new DeleteEnhanceYourStayCategoryItemExtraRequest(@in), ct);
    }
    private async Task<IResult> CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraRequest(@in), ct);

    #endregion

}
