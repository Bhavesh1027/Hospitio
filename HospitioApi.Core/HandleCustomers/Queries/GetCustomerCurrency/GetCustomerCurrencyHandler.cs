using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomerCurrency;
public record GetCustomerCurrencyByIdRequest(GetCustomerCurrencyIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerCurrencyHandler : IRequestHandler<GetCustomerCurrencyByIdRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerCurrencyHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerCurrencyByIdRequest request, CancellationToken cancellationToken)
    {
        var customercurrency = await _db.Customers.Where(c => c.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);

        if (customercurrency == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        var customerCurrencyById = new CustomerCurrencyByIdOut
        {
            CurrencyCode = customercurrency.CurrencyCode,
        };

        return _response.Success(new GetCustomerCurrencyOut("Get customer currency successful.", customerCurrencyById));
    }
}
