using MediatR;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using System.Data;

namespace HospitioApi.Core.HandleTaxiTransfer.Queries.GetTaxiTransferMonths;
public record GetTaxiTransferMonthsRequest(GetTaxiTransferMonthsIn In) : IRequest<AppHandlerResponse>;
public class GetTaxiTransferMonthsHandler : IRequestHandler<GetTaxiTransferMonthsRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly ApplicationDbContext _db;
    public GetTaxiTransferMonthsHandler(IHandlerResponseFactory response, ApplicationDbContext db)
    {
        _response = response;
        _db = db;
    }
    public async Task<AppHandlerResponse> Handle(GetTaxiTransferMonthsRequest request, CancellationToken cancellationToken)
    {
        var months = _db.TaxiTransferGuestRequests
                      .Where(s => s.DeletedAt == null && s.IsMonthlyReport == true && s.PickUpDate.Value.Year == int.Parse(request.In.year))
                      .Select(s => s.PickUpDate.Value.Month)
                      .Distinct()
                      .OrderBy(month => month)
                      .ToList();

        var taxiTransferMonths = new TaxiTransferMonths
        {
            MonthNumber = months
        };

        return _response.Success(new GetTaxiTransferMonthsOut("Get Taxi Transfer Months Data Successful.", taxiTransferMonths));
    }

}
