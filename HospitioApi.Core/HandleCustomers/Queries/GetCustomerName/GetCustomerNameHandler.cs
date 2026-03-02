using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerCurrency;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomerName;

public record GetCustomerNameRequest(GetCustomerNameIn In) : IRequest<AppHandlerResponse>;

public class GetCustomerNameHandler : IRequestHandler<GetCustomerNameRequest,AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerNameHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerNameRequest request, CancellationToken cancellationToken)
    {
        var customerName = await _db.Customers.Where(c => c.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);

        if (customerName == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        var CustomerNameClass = new GetCustomerNameClass
        {
            BusinessName = customerName.BusinessName
        };

        return _response.Success(new GetCustomerNameOut("Get customerName successful.", CustomerNameClass));
    }
}

