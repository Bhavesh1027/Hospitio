using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleVonageSMS.Commands.UpdateCustomerVonageNumber
{
    public record UpdateCustomerVonageNumberHandlerRequest(UpdateCustomerVonageNumberIn In)
 : IRequest<AppHandlerResponse>;
    public class UpdateCustomerVonageNumberHandler : IRequestHandler<UpdateCustomerVonageNumberHandlerRequest, AppHandlerResponse>
    {
        private readonly ApplicationDbContext _db;
        private readonly IHandlerResponseFactory _response;
        private readonly IVonageService _vonageService;

        public UpdateCustomerVonageNumberHandler(ApplicationDbContext db,
        IHandlerResponseFactory response, IVonageService vonageService)
        {
            _db = db;
            _response = response;
            _vonageService = vonageService;
        }
        public async Task<AppHandlerResponse> Handle(UpdateCustomerVonageNumberHandlerRequest request, CancellationToken cancellationToken)
        {
            var VonageDetails = await _db.VonageCredentials.Where(i => i.CustomerId == request.In.CustomerId).FirstOrDefaultAsync();

            if (VonageDetails == null)
            {
               return _response.Error("Vonage Credential Not Available", AppStatusCodeError.Gone410);
            }

            string vonageApiKey = VonageDetails.APIKey;
            string vonageApiSecret = VonageDetails.APISecret;
            string vonageAppId = VonageDetails.AppId;

            var response = await _vonageService.UpdateNumber(vonageApiKey, vonageApiSecret, request.In.Country, request.In.Number, vonageAppId);

            var CustomerData = await _db.Customers.Where(i => i.Id == request.In.CustomerId).FirstOrDefaultAsync();
            if (response.ErrorCode != "200")
            {
                return _response.Error("Data Not Available", AppStatusCodeError.InternalServerError500);
            }

            CustomerData.IsTwoWayComunication = true;
            CustomerData.PhoneCountry = request.In.Country;
            CustomerData.PhoneNumber = $"+{request.In.Number}";

            await _db.SaveChangesAsync(cancellationToken);

            return _response.Success(new UpdateCustomerVonageNumberOut("Number Update successfully"));
        }
    }
}
