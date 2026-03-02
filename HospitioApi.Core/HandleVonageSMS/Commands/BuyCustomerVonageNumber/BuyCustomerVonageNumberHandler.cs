using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleVonageSMS.Commands.BuyCustomerVonageNumber
{
    public record BuyCustomerVonageNumberHandlerRequest(BuyCustomerVonageNumberIn In)
  : IRequest<AppHandlerResponse>;
    public class BuyCustomerVonageNumberHandler : IRequestHandler<BuyCustomerVonageNumberHandlerRequest, AppHandlerResponse>
    {
        private readonly ApplicationDbContext _db;
        private readonly IHandlerResponseFactory _response;
        private readonly IVonageService _vonageService;

        public BuyCustomerVonageNumberHandler(ApplicationDbContext db,
        IHandlerResponseFactory response,IVonageService vonageService)
        {
            _db = db;
            _response = response;
            _vonageService = vonageService;
        }
        public async Task<AppHandlerResponse> Handle(BuyCustomerVonageNumberHandlerRequest request, CancellationToken cancellationToken)
        {
            var VonageDetails = await _db.VonageCredentials.Where(i => i.CustomerId == request.In.CustomerId).FirstOrDefaultAsync();
            if (VonageDetails == null || VonageDetails.APIKey == null ||VonageDetails.APISecret == null)
            {
                return _response.Error("Vonage Credential Not Available", AppStatusCodeError.Gone410);
            }

            string vonageApiKey = VonageDetails.APIKey;
            string vonageApiSecret = VonageDetails.APISecret;

            var response = await _vonageService.BuyNumber(vonageApiKey, vonageApiSecret, request.In.Country, request.In.Number);

            var CustomerData = await _db.Customers.Where(i => i.Id == request.In.CustomerId).FirstOrDefaultAsync();
            if (response.ErrorCode != "200")
            {
                return _response.Error("Data Not Available", AppStatusCodeError.InternalServerError500);
            }

            CustomerData.IsTwoWayComunication = true;
            CustomerData.PhoneCountry = request.In.Country;
            CustomerData.PhoneNumber = $"+{request.In.Number}";

            await _db.SaveChangesAsync(cancellationToken);
            
            return _response.Success(new BuyCustomerVonageNumberOut("Number Buy successfully"));

        }
    }
}
