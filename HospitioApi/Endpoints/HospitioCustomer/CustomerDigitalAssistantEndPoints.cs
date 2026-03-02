using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.CreateCustomersDigitalAssistants;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.DeleteCustomersDigitalAssistants;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.UpdateCustomersDigitalAssistants;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.UpdateIsActiveCustomersDigitalAssistants;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistants;
using HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistantsById;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer
{
    public class CustomerDigitalAssistantEndPoints : IEndpointsModule
    {
        public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-customer/digitalassistants",
        singular: "api/hospitio-customer/digitalassistant");
        public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
        {
        app.MapGet($"/{Route.Plural}",GetCustomerDigitalAssistantsAsync)
        .RequireAuthorization(),
        app.MapGet($"/{Route.Singular}",GetCustomerDigitalAssistantAsync)
        .RequireAuthorization(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerDigitalAssistantAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/create", CreateCustomerDigitalAssistantAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/updateIsActive", UpdateIsActiveCustomerDigitalAssistantAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/update", UpdateCustomerDigitalAssistantAsync)
        .RequireAuthorization()
        };

        #region Delegates
        private async Task<IResult> GetCustomerDigitalAssistantsAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        {
            GetCustomersDigitalAssistantsIn @in = new()
            {
                //SearchColumn = SearchColumn,
                //SearchValue = SearchValue,
                //PageNo = PageNo,
                //PageSize = PageSize,
                //SortColumn = SortColumn,
                //SortOrder = SortOrder,
                CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"))
            };
            return await mtrHlpr.ToResultAsync(new GetCustomersDigitalAssistantsRequest(@in), ct);

        }
        private async Task<IResult> GetCustomerDigitalAssistantAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "Id")] int Id, CT ct)
        {
            GetCustomersDigitalAssistantsByIdIn @in = new() { Id = Id };
            return await mtrHlpr.ToResultAsync(new GetCustomersDigitalAssistantsByIdRequest(@in), ct);
        }
        private async Task<IResult> DeleteCustomerDigitalAssistantAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int Id, CT ct)
        {
            DeleteCustomersDigitalAssistantsIn @in = new() { Id = Id };
            return await mtrHlpr.ToResultAsync(new DeleteCustomersDigitalAssistantsRequest(@in), ct);
        }
        private async Task<IResult> CreateCustomerDigitalAssistantAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateCustomersDigitalAssistantsIn @in, ClaimsPrincipal cp, CT ct)
        {
            @in.CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"));
            return await mtrHlpr.ToResultAsync(new CreateCustomersDigitalAssistantsRequest(@in), ct);
        }

        private async Task<IResult> UpdateCustomerDigitalAssistantAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] UpdateCustomersDigitalAssistantsIn @in, CT ct)
            => await mtrHlpr.ToResultAsync(new UpdateCustomersDigitalAssistantsRequest(@in), ct);
        private async Task<IResult> UpdateIsActiveCustomerDigitalAssistantAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] UpdateIsActiveCustomersDigitalAssistantsIn @in, CT ct)
    => await mtrHlpr.ToResultAsync(new UpdateIsActiveCustomersDigitalAssistantsRequest(@in), ct);
        #endregion
    }

}
