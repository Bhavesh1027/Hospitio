using MediatR;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTaxiTransfer.Queries.GetTaxiTransferYears;
public record GetTaxiTransferYearsRequest() : IRequest<AppHandlerResponse>;
public class GetTaxiTransferYearsHandler : IRequestHandler<GetTaxiTransferYearsRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly ApplicationDbContext _db;
    public GetTaxiTransferYearsHandler(IHandlerResponseFactory response, ApplicationDbContext db)
    {
        _response = response;
        _db = db;
    }
    public async Task<AppHandlerResponse> Handle(GetTaxiTransferYearsRequest request, CancellationToken cancellationToken)
    {
        var Years = _db.TaxiTransferGuestRequests
                      .Where(s => s.DeletedAt == null && s.IsMonthlyReport == true)
                      .Select( s => s.PickUpDate.Value.Year)
                      .Distinct()
                      .OrderBy(Year => Year)
                      .ToList();

        var taxiTransferYears = new TaxiTransferYears
        {
            Years = Years
        };

        return _response.Success(new GetTaxiTransferYearsOut("Get Taxi Transfer Year Data Successful.", taxiTransferYears));
    }
}
