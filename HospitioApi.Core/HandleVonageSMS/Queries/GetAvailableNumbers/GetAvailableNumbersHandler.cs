using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleVonageSMS.Queries.GetAvailableNumbers;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Text.Json;
using Vonage.Numbers;

namespace HospitioApi.Core.HandleVonageSMS.Commands.GetAvailableNumbers
{
    public record GetAvailableNumbersHandlerRequest(GetAvailableNumbersIn In)
    : IRequest<AppHandlerResponse>;
    public class GetAvailableNumbersHandler : IRequestHandler<GetAvailableNumbersHandlerRequest, AppHandlerResponse>
    {
        private readonly ApplicationDbContext _db;
        private readonly IHandlerResponseFactory _response;
        private readonly IVonageService _vonageService;

        public GetAvailableNumbersHandler(ApplicationDbContext db,
        IHandlerResponseFactory response, IVonageService vonageService)
        {
            _db = db;
            _response = response;
            _vonageService =  vonageService;
        }
        public async Task<AppHandlerResponse> Handle(GetAvailableNumbersHandlerRequest request, CancellationToken cancellationToken)
        {
            var VonageDetails = await _db.VonageCredentials.Where(i => i.CustomerId == request.In.CustomerId).FirstOrDefaultAsync();

            if (VonageDetails == null || VonageDetails.APIKey == null ||VonageDetails.APISecret == null)
            {
                return _response.Error("Vonage Credential; Not Available", AppStatusCodeError.Gone410);
            }

            string vonageApiKey = VonageDetails.APIKey;
            string vonageApiSecret = VonageDetails.APISecret;

            var response = await _vonageService.SearchNumbers(vonageApiKey, vonageApiSecret, request.In.country, request.In.type, request.In.features, request.In.pattern, (SearchPattern)(request.In.search_pattern),request.In.size, request.In.index);

            string json = JsonSerializer.Serialize<NumbersSearchResponse>(response);

            if (response.Numbers == null)
            {
                return _response.Error("Data Not Available", AppStatusCodeError.Gone410);
            }
            return _response.Success(new GetAvailableNumbersOut("Numbers get successfully", json));
        }

    }
}
