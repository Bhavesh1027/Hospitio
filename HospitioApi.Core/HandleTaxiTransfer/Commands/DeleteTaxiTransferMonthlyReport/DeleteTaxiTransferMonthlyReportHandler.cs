using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleTaxiTransfer.Commands.DeleteTaxiTransferMonthlyReport;
public record DeleteTaxiTransferMonthlyReportRequest(DeleteTaxiTransferMonthlyReportIn In) : IRequest<AppHandlerResponse>;
public class DeleteTaxiTransferMonthlyReportHandler : IRequestHandler<DeleteTaxiTransferMonthlyReportRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DeleteTaxiTransferMonthlyReportHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(DeleteTaxiTransferMonthlyReportRequest request, CancellationToken cancellationToken)
    {

        var TaxiTrasferMonthlyData = await _db.TaxiTransferGuestRequests.Where(s => s.PickUpDate.Value.Month == int.Parse(request.In.Month) && s.PickUpDate.Value.Year == int.Parse(request.In.Year)).ToListAsync(cancellationToken);

        if (TaxiTrasferMonthlyData == null || TaxiTrasferMonthlyData.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        foreach (var taxiData in TaxiTrasferMonthlyData)
        {
            taxiData.IsMonthlyReport = false;
        }
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new DeleteTaxiTransferMonthlyReportOut("Delete TaxiTransfer Monthly Report Successful."));

    }
}
