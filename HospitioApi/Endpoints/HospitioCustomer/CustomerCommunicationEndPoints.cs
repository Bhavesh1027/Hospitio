using Microsoft.AspNetCore.Mvc;
using HospitioApi.Helpers;
using System.Security.Claims;
using HospitioApi.Core.HandlerCustomerCommunication.Queries.GetCustomerCommunication;
using HospitioApi.Core.HandlerCustomerCommunication.Queries.GetCustomerCommunicationByReservationId;

namespace HospitioApi.Endpoints.HospitioCustomer
{
    public class CustomerCommunicationEndPoints : IEndpointsModule
    {
        public AppRoute Route { get; init; } = new(
            plural__: "api/hospitio-customer/communications",
            singular: "api/hospitio-customer/communication");
        public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
        {
            app.MapGet($"/{Route.Plural}/Search",GetCustomerCommunicationsAsync)
            .RequireAuthorization(),

            app.MapGet($"/{Route.Singular}", GetByReservationIdAsync)
            .RequireAuthorization()
        };
        #region Delegates
        private async Task<IResult> GetCustomerCommunicationsAsync([FromServices] IMediatorHelper mtrHlpr, string? SearchValue, int PageNo, int PageSize, ClaimsPrincipal cp, CT ct)
        {
            GetCustomerCommunicationIn @in = new()
            {
                CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId")),
                SearchString = SearchValue,
                PageNo = PageNo,
                PageSize = PageSize,
                UserLevel = (cp.FindFirstValue("CustomerUserLevel")),
                CustomerUserId = Convert.ToInt32(cp.FindFirstValue("UserId"))
            };
            return await mtrHlpr.ToResultAsync(new GetCustomerCommunicationRequest(@in), ct);
        }
        private async Task<IResult> GetByReservationIdAsync([FromServices] IMediatorHelper mtrHlpr, CT ct, int Id, int? ReservationId)
        {
            GetCustomerCommunicationByReservationIdIn @in = new()
            {
                Id = Id,
                ReservationId = ReservationId
            };
            return await mtrHlpr.ToResultAsync(new GetCustomerCommunicationByReservationIdRequest(@in), ct);
        }
        #endregion
    }
}
