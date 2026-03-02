using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleTaxiTransfer.Commands.DeleteTaxiTransferMonthlyReport;
using HospitioApi.Core.HandleTaxiTransfer.Queries.GetAllTransferData;
using HospitioApi.Core.HandleTaxiTransfer.Queries.GetDownloadTransferData;
using HospitioApi.Core.HandleTaxiTransfer.Queries.GetTaxiTransferMonths;
using HospitioApi.Core.HandleTaxiTransfer.Queries.GetTaxiTransferYears;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class TaxiTransferRequest : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
    plural__: "api/hospitio-admin/taxiTransfers",
    singular: "api/hospitio-admin/taxiTransfer");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}/getTaxiTransferData", GetTaxiTransferDataAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Plural}/downloadTaxiTransferData", GetDownloadTaxiTransferDataAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Plural}/getTaxiTransferMonthData", GetTaxiTransferMonthDataAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Plural}/deleteTaxiTransferMonthlyReport", DeleteTaxiTransferMonthlyReportAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Plural}/getTaxiTransferYearData", GetTaxiTransferYearDataAsync)
        .AllowAnonymous(),
    };

    private async Task<IResult> GetTaxiTransferDataAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct, string? searchValue, int pageNo, int pageSize , int? CustomerId , int? GuestId , DateTime? FromCreateAt, DateTime? ToCreateAt)
    {
        GetAllTransferDataIn @in = new()
        {
            SearchValue = searchValue,
            PageNo = pageNo,
            PageSize = pageSize,
            CustomerId = CustomerId,
            GuestId = GuestId,
            ToCreateAt = ToCreateAt,
            FromCreateAt = FromCreateAt
        };
        return await mtrHlpr.ToResultAsync(new GetAllTransferDataRequest(@in), ct);
    }
    private async Task<IResult> GetDownloadTaxiTransferDataAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, string? month, string? year ,CT ct)
    {
        GetDownloadTransferDataIn @in = new()
        {
            Month = month,
            Year = year,
        };
        return await mtrHlpr.ToResultAsync(new GetDownloadTransferDataRequest(@in), ct);
    }
    private async Task<IResult> GetTaxiTransferMonthDataAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp,string? year ,CT ct)
    {
        GetTaxiTransferMonthsIn @in = new()
        {
            year = year,
        };
        return await mtrHlpr.ToResultAsync(new GetTaxiTransferMonthsRequest(@in), ct);
    }
    private async Task<IResult> DeleteTaxiTransferMonthlyReportAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] DeleteTaxiTransferMonthlyReportIn @in , CT ct)
    {
        return await mtrHlpr.ToResultAsync(new DeleteTaxiTransferMonthlyReportRequest(@in), ct);
    }
    private async Task<IResult> GetTaxiTransferYearDataAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new GetTaxiTransferYearsRequest(), ct);
    }

}
