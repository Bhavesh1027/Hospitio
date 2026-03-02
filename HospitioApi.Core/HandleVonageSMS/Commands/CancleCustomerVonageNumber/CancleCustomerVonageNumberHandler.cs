using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleVonageSMS.Commands.CancleCustomerVonageNumber
{
    public record CancleCustomerVonageNumberHandlerRequest(CancleCustomerVonageNumberIn In)
 : IRequest<AppHandlerResponse>;
    public class CancleCustomerVonageNumberHandler : IRequestHandler<CancleCustomerVonageNumberHandlerRequest, AppHandlerResponse>
    {
        private readonly ApplicationDbContext _db;
        private readonly IHandlerResponseFactory _response;
        private readonly IVonageService _vonageService;

        public CancleCustomerVonageNumberHandler(ApplicationDbContext db,
        IHandlerResponseFactory response, IVonageService vonageService)
        {
            _db = db;
            _response = response;
            _vonageService = vonageService;
        }
        public async Task<AppHandlerResponse> Handle(CancleCustomerVonageNumberHandlerRequest request, CancellationToken cancellationToken)
        {
            var VonageDetails = await _db.VonageCredentials.Where(i => i.CustomerId == request.In.CustomerId).FirstOrDefaultAsync();
            if (VonageDetails == null)
            {
                return _response.Error("Vonage Credential Not Available", AppStatusCodeError.Gone410);
            }

            string vonageApiKey = VonageDetails.APIKey;
            string vonageApiSecret = VonageDetails.APISecret;

            var response = await _vonageService.CancelNumber(vonageApiKey, vonageApiSecret, request.In.Country, request.In.Number);

            if (response.ErrorCode != "200")
            {
                return _response.Error("Data Not Available", AppStatusCodeError.InternalServerError500);
            }

            return _response.Success(new CancleCustomerVonageNumberOut("Number Cancel successfully"));
        }
    }
}
