using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleCustomers.Commands.CreateCustomer;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomerName;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerGuestToken;
public record GetCustomerGuestTokenRequest(GetCustomerGuestTokenIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerGuestTokenHandler : IRequestHandler<GetCustomerGuestTokenRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerGuestTokenHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerGuestTokenRequest request, CancellationToken cancellationToken)
    {
        var customerGuestId = CryptoExtension.Decrypt(request.In.Id, (UserTypeEnum.Guest).ToString());
        var customerGuest = await _db.CustomerGuests.Where(c => c.Id == int.Parse(customerGuestId)).FirstOrDefaultAsync(cancellationToken);

        if (customerGuest == null)
        {
            return _response.Error("CustomerGuest not found", AppStatusCodeError.Gone410);
        }

        var CustomerGuestToken = new GetCustomerGuestTokenClass
        {
            CustomerGuestToken = customerGuest.GuestToken
        };

        return _response.Success(new GetCustomerGuestTokenOut("Get CustomerGuest Token successful.", CustomerGuestToken));
    }
}

