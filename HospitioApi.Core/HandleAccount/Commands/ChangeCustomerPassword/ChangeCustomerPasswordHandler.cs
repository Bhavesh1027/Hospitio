using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleCustomers.Commands.CreateCustomer;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleAccount.Commands.ChangeCustomerPassword;

public record ChangeCustomerPasswordHandlerRequest(ChangeCustomerPasswordIn In, int customerId)
    : IRequest<AppHandlerResponse>;

public class ChangeCustomerPasswordHandler : IRequestHandler<ChangeCustomerPasswordHandlerRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public ChangeCustomerPasswordHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(ChangeCustomerPasswordHandlerRequest request, CancellationToken cancellationToken)
    {
        var customerUser = await _db.CustomerUsers.Where(x => x.CustomerId == request.customerId).FirstOrDefaultAsync(cancellationToken);

        if (customerUser != null && customerUser.Password != null)
        {
            var oldEncryptedPassword = CryptoExtension.Encrypt(request.In.OldPassword, customerUser.Id.ToString());

            if (oldEncryptedPassword == customerUser.Password)
            {
                var newEncryptedPassword = CryptoExtension.Encrypt(request.In.NewPassword, customerUser.Id.ToString());

                customerUser.Password = newEncryptedPassword;

                await _db.SaveChangesAsync(cancellationToken);

                return _response.Success(new ChangeCustomerPasswordOut("New Password Updated Successful"));

            }
        }

        return _response.Error("Old Password Is Invalid.", AppStatusCodeError.Forbidden403, skipEmailNotification: true);
    }
}
