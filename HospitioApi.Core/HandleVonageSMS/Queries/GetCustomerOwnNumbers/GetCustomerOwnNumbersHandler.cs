using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Text.Json;
using Vonage.Numbers;

namespace HospitioApi.Core.HandleVonageSMS.Queries.GetCustomerOwnNumbers
{
    public record GetCustomerOwnNumbersHandlerRequest(GetCustomerOwnNumbersIn In)
    : IRequest<AppHandlerResponse>;
    public class GetCustomerOwnNumbersHandler : IRequestHandler<GetCustomerOwnNumbersHandlerRequest, AppHandlerResponse>
    {
        private readonly ApplicationDbContext _db;
        private readonly IHandlerResponseFactory _response;
        private readonly IVonageService _vonageService;

        public GetCustomerOwnNumbersHandler(ApplicationDbContext db,
        IHandlerResponseFactory response, IVonageService vonageService)
        {
            _db = db;
            _response = response;
            _vonageService  = vonageService;
        }
        public async Task<AppHandlerResponse> Handle(GetCustomerOwnNumbersHandlerRequest request, CancellationToken cancellationToken)
        {
            var VonageDetails = await _db.VonageCredentials.Where(i => i.CustomerId == request.In.CustomerId).FirstOrDefaultAsync();

            if (VonageDetails == null || VonageDetails.APIKey == null || VonageDetails.APISecret == null)
            {
               return _response.Error("Vonage Credential Not Available", AppStatusCodeError.Gone410);
            }

            string vonageApiKey = VonageDetails.APIKey;
            string vonageApiSecret = VonageDetails.APISecret;   
      
            var response = await _vonageService.ListOwnedNumbers(vonageApiKey, vonageApiSecret, request.In.pattern, (SearchPattern)(request.In.search_pattern));

            string JsonData = JsonSerializer.Serialize<NumbersSearchResponse>(response);

            return _response.Success(new GetCustomerOwnNumbersOut("Numbers  get successfully", JsonData));

        }
    }
}
