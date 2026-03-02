
using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleVonageRecordsLogs.Commands.CreateVonageRecordsReport;
using HospitioApi.Core.HandleVonageRecordsLogs.Commands.GetVonageRecordReport;
using HospitioApi.Core.HandleVonageRecordsLogs.Commands.GetVonageRecordsStatus;
using HospitioApi.Helpers;
using System.Security.Claims;


namespace HospitioApi.Endpoints.HospitioAdmin
{
    public class VonageRecordLogsEndpoint : IEndpointsModule
    {
        public AppRoute Route { get; init; } = new(null, singular: "api/hospitio-admin/vonagerecords");
        public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
        {   
            app.MapPost($"/{Route.Singular}",GetVonageReportsFile)
            .RequireAuthorization(),
            app.MapPost($"/{Route.Singular}/recordstatus",GetVonageReportStatusAsync)
            .AllowAnonymous(),
            app.MapPost($"/{Route.Singular}/getrecords",GetVonageRecordsJsonAsyn)
            .RequireAuthorization(),
        };

        #region

        private async Task<IResult> GetVonageReportsFile([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateVonageRecordsReportIn @in, CT ct)
        {
            return await mtrHlpr.ToResultAsync(new CreateVonageRecordsReportRequest(@in), ct);
        }
        private async Task<IActionResult> GetVonageReportStatusAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] GetVonageRecordsStatusIn @in, CT ct)
        {
            await mtrHlpr.ToResultAsync(new GetVonageRecordsStatusRequest(@in), ct);
            return new OkResult();
        }
        private async Task<IResult> GetVonageRecordsJsonAsyn([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] GetVonageRecordReportIn @in, CT ct)
        {
            return await mtrHlpr.ToResultAsync(new GetVonageRecordReportRequest(@in), ct);
        }
        #endregion
    }
}
