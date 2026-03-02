using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerEnhanceYourStay.Queries.GetCustomerEnhanceYourStay;
using HospitioApi.Core.HandleCustomerEnhanceYourStay.Queries.GetCustomerEnhanceYourStayByCategory;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Queries.GetCustomerEnhanceYourStayItemById;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest
{
    public class CustomerEnhanceYourStayEndPoint : IEndpointsModule
    {
        public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-guest/enhance-stays",
       singular: "api/hospitio-guest/enhance-stay");
        public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
        {
        app.MapGet($"/{Route.Plural}", GetCustomerEnhanceYourStayAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}",GetCustomerEnhanceYourStayItemAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Plural}bycategory",GetCustomerEnhanceYourStayItemByCategoryAsync)
        .AllowAnonymous(),
        //app.MapDelete($"/{Route.Singular}", DeleteCustomerEnhanceYourStayAsync)
        //.RequireAuthorization(),
        //app.MapPost($"/{Route.Singular}/create", CreateCustomerEnhanceYourStayAsync)
        //.RequireAuthorization()
        };

        #region Delegates
        private async Task<IResult> GetCustomerEnhanceYourStayAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "BuilderId")] int BuilderId, ClaimsPrincipal cp, CT ct)
        {
            GetCustomerEnhanceYourStayIn @in = new()
            {
                BuilderId = BuilderId
            };
            return await mtrHlpr.ToResultAsync(new GetCustomerEnhanceYourStayRequest(@in,cp.UserType()), ct);

        }
        private async Task<IResult> GetCustomerEnhanceYourStayItemAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "EnhanceYourStayItemId")] int EnhanceYourStayItemId, CT ct)
        {
            GetCustomerEnhanceYourStayItemByIdIn @in = new() { Id = EnhanceYourStayItemId };
            return await mtrHlpr.ToResultAsync(new GetCustomerEnhanceYourStayItemByIdRequest(@in, cp.UserType()), ct);
        }
        private async Task<IResult> GetCustomerEnhanceYourStayItemByCategoryAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "CategoryId")] int CategoryId, CT ct)
        {
            GetCustomerEnhanceYourStayByCategoryIn @in = new() { CategoryId = CategoryId };
            return await mtrHlpr.ToResultAsync(new GetCustomerEnhanceYourStayByCategoryRequest(@in), ct);
        }
        //private async Task<IResult> DeleteCustomerEnhanceYourStayAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int CategoryId, CT ct)
        //{
        //    DeleteCustomerEnhanceYourStayIn @in = new() { CategoryId = CategoryId };
        //    return await mtrHlpr.ToResultAsync(new DeleteCustomerEnhanceYourStayRequest(@in), ct);
        //}
        //private async Task<IResult> CreateCustomerEnhanceYourStayAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateCustomerEnhanceYourStayIn @in, ClaimsPrincipal cp, CT ct)
        //{
        //    @in.CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"));
        //    return await mtrHlpr.ToResultAsync(new CreateCustomerEnhanceYourStayRequest(@in), ct);
        //}
        #endregion
    }
}
