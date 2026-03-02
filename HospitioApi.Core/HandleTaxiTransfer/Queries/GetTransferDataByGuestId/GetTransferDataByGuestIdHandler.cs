using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;
using System.Net.Http.Headers;
using System.Text;

namespace HospitioApi.Core.HandleTaxiTransfer.Queries.GetTransferDataByGuestId;
public record GetTransferDataByGuestRequest(GetTransferDataByGuestIdIn In) : IRequest<AppHandlerResponse>;
public class GetTransferDataByGuestIdHandler : IRequestHandler<GetTransferDataByGuestRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;
    private readonly IJwtService _jwtService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Gr4vyApiSettingsOptions _gr4VyApiSettingsOptions;
    private readonly ApplicationDbContext _db;
    private readonly WelComePickUpsSettingsOptions _welComePickUpsSettings;
    public GetTransferDataByGuestIdHandler(IHandlerResponseFactory response, IDapperRepository dapper, IJwtService jwtService, IHttpClientFactory httpClientFactory, IOptions<Gr4vyApiSettingsOptions> gr4VyApiSettingsOptions, ApplicationDbContext db, IOptions<WelComePickUpsSettingsOptions> welComePickUpsSettings)
    {
        _response = response;
        _dapper = dapper;
        _jwtService = jwtService;
        _httpClientFactory = httpClientFactory;
        _gr4VyApiSettingsOptions = gr4VyApiSettingsOptions.Value;
        _db = db;
        _welComePickUpsSettings = welComePickUpsSettings.Value;
    }
    public async Task<AppHandlerResponse> Handle(GetTransferDataByGuestRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerId", int.Parse(request.In.CustomerId), DbType.Int32);
        spParams.Add("GuestId", int.Parse(request.In.GuestId), DbType.Int32);

        var transferResponses = await _dapper.GetAll<TransferResponse>("[dbo].[GetTaxiTransferDataByGuestId]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (transferResponses == null || transferResponses.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        foreach (var response in transferResponses)
        {
            if (response.TransferJson != null)
            {
                response.transferModel = JsonConvert.DeserializeObject<TransferModel>(response.TransferJson);
            }
            if(response.ExtraDetailsJson != null)
            {
                response.ExtraDetails = JsonConvert.DeserializeObject<ExtraDetails>(response.ExtraDetailsJson);
            }
        }

        return _response.Success(new GetTransferDataByGuestIdOut("Get Guest Taxi Transfer Data Successful.", transferResponses));
    }

    //private async Task ProcessTransfer(TransferResponse response, CancellationToken cancellationToken)
    //{
    //    if (response.TransferStatus == "confirmed")
    //    {
    //        using var httpClient = _httpClientFactory.CreateClient();

    //        var showTransferApiUrl = _welComePickUpsSettings.WelComePickUps_URL + $"v1/external/transfers/{response.TransferId}";
    //        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_welComePickUpsSettings.WelComePickUps_APIKey}");

    //        var showTransferDetailResponse = await httpClient.GetAsync(showTransferApiUrl, cancellationToken);

    //        if (showTransferDetailResponse.IsSuccessStatusCode)
    //        {
    //            var responseBody = await showTransferDetailResponse.Content.ReadAsStringAsync();
    //            var responseObject = JsonConvert.DeserializeObject<TransferModel>(responseBody);

    //            if (responseObject != null && responseObject.data.attributes.transfer_status != response.TransferStatus)
    //            {
    //                await UpdateTransferStatus(response, responseBody, responseObject, cancellationToken);
    //            }
    //        }
    //    }

    //    if (response.RefundStatus == "processing" && response.RefundId != null)
    //    {
    //        using var gr4VyHttpClient = _httpClientFactory.CreateClient();

    //        var apiUrl = $"{_gr4VyApiSettingsOptions.BaseUrl}transactions/{response.GRPaymentId}/refunds/{response.RefundId}";
    //        var token = _jwtService.GenerateJWTokenForGr4vy();

    //        gr4VyHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //        var detailResponse = await gr4VyHttpClient.GetAsync(apiUrl, cancellationToken);

    //        if (detailResponse.IsSuccessStatusCode)
    //        {
    //            var responseBody = await detailResponse.Content.ReadAsStringAsync();
    //            var jsonResponse = JObject.Parse(responseBody);

    //            await UpdateRefundStatus(response, jsonResponse, cancellationToken);
    //        }
    //    }

    //    if (response.TransferJson != null)
    //    {
    //        response.transferModel = JsonConvert.DeserializeObject<TransferModel>(response.TransferJson);
    //    }

    //    if(response.ExtraDetailsJson != null)
    //    {
    //        response.ExtraDetails = JsonConvert.DeserializeObject<ExtraDetails>(response.ExtraDetailsJson);
    //    }
    //}


    //private async Task UpdateTransferStatus(TransferResponse response, string responseBody, TransferModel responseObject, CancellationToken cancellationToken)
    //{
    //    var taxiTransfer = await _db.TaxiTransferGuestRequests.FirstOrDefaultAsync(t => t.Id == response.Id, cancellationToken);
    //    if (taxiTransfer != null)
    //    {
    //        taxiTransfer.TransferStatus = responseObject.data.attributes.transfer_status;
    //        taxiTransfer.TransferJson = responseBody;

    //        if (taxiTransfer.RefundId == null && responseObject.data.attributes.transfer_status != "operated")
    //        {
    //            using var gr4VyHttpClient = _httpClientFactory.CreateClient();

    //            var captureApiUrl = $"{_gr4VyApiSettingsOptions.BaseUrl}transactions/{response.GRPaymentId}/refunds";
    //            var requestBody = new { amount = Convert.ToInt32((int.Parse(responseObject.data.attributes.fare.refund.policy.refund_percentage) / 100) * taxiTransfer.HospitioFareAmount) };

    //            var json = JsonConvert.SerializeObject(requestBody);
    //            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

    //            gr4VyHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtService.GenerateJWTokenForGr4vy());
    //            var detailResponse = await gr4VyHttpClient.PostAsync(captureApiUrl, httpContent, cancellationToken);

    //            if (detailResponse.IsSuccessStatusCode)
    //            {
    //                var refundResponseBody = await detailResponse.Content.ReadAsStringAsync();
    //                dynamic jsonResponse = JObject.Parse(refundResponseBody);

    //                taxiTransfer.GRPaymentDetails = refundResponseBody;
    //                taxiTransfer.RefundId = jsonResponse.id;
    //                taxiTransfer.RefundStatus = jsonResponse.status;

    //                response.GRPaymentDetails = refundResponseBody;
    //                response.RefundId = jsonResponse.id;
    //                response.RefundStatus = jsonResponse.status;

    //                taxiTransfer.RefundAmount = Convert.ToInt32((int.Parse(responseObject.data.attributes.fare.refund.policy.refund_percentage) / 100) * taxiTransfer.HospitioFareAmount);
    //                response.RefundAmount = taxiTransfer.RefundAmount;
    //            }
    //        }

    //        await _db.SaveChangesAsync(cancellationToken);
    //    }

    //    response.TransferStatus = responseObject.data.attributes.transfer_status;
    //    response.TransferJson = responseBody;
    //}

    //private async Task UpdateRefundStatus(TransferResponse response, dynamic jsonResponse, CancellationToken cancellationToken)
    //{
    //    var taxiTransfer = await _db.TaxiTransferGuestRequests.FirstOrDefaultAsync(t => t.Id == response.Id, cancellationToken);
    //    if (taxiTransfer != null)
    //    {
    //        taxiTransfer.RefundStatus = jsonResponse.status;
    //        await _db.SaveChangesAsync(cancellationToken);

    //        response.RefundStatus = jsonResponse.status;
    //    }
    //}

}
