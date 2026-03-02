using ClosedXML.Excel;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.DownloadCustomerGuest;
public record DownloadCustomerGuestsRequest(int CustomerId) : IRequest<AppHandlerResponse>;
public class DownloadCustomerGuestHandler : IRequestHandler<DownloadCustomerGuestsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly IUserFilesService _userFilesService;
    private readonly FrontEndLinksSettingsOptions _frontEndLinksSettings;

    public DownloadCustomerGuestHandler(IDapperRepository dapper, IHandlerResponseFactory response, IUserFilesService userFilesService , IOptions<FrontEndLinksSettingsOptions> frontEndLinksSettings)
    {
        _dapper = dapper;
        _response = response;
        _userFilesService = userFilesService;
        _frontEndLinksSettings = frontEndLinksSettings.Value;
    }

    public async Task<AppHandlerResponse> Handle(DownloadCustomerGuestsRequest request, CancellationToken cancellationToken)
    {
        var downloadGuest = new DownloadCustomerGuestsOut();

        var spParams = new DynamicParameters();
        spParams.Add("CustomerId", request.CustomerId, DbType.Int32);
        spParams.Add("SearchValue", "", DbType.String);
        spParams.Add("PageNo", 1, DbType.Int32);
        spParams.Add("PageSize", int.MaxValue, DbType.Int32);
        spParams.Add("SortColumn", "", DbType.String);
        spParams.Add("SortOrder", "", DbType.String);

        var result = await _dapper
            .GetAll<CustomerGuestDownloadOut>("[dbo].[GetCustomerGuests]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        var updatedResult = result.ToList().Select(item => new CustomerGuestDownloadOut
        {
            Firstname = item.Firstname,
            Lastname = item.Lastname,
            RoomNumber = item.RoomNumber,
            CheckinDate = item.CheckinDate,
            CheckoutDate = item.CheckoutDate,
            GuestToken = _frontEndLinksSettings.GuestPortal + "?id=" + item.GuestToken
        }).ToList();

        if (result == null || result.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Data"); // Sheet name

            // Add headers
            int col = 1;
            worksheet.Cell(1, col).Value = "Serial Number";
            worksheet.Cell(1, col).Style.Font.Bold = true;
            worksheet.Cell(1, col).Style.Fill.BackgroundColor = XLColor.LightGray;
            col++;
            foreach (var prop in typeof(CustomerGuestDownloadOut).GetProperties())
            {
                var headerCell = worksheet.Cell(1, col);
                headerCell.Value = prop.Name;
                if(prop.Name == "GuestToken")
                {
                    headerCell.Value = "GuestUrl";
                }
                // Apply CSS styling to the header cell
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Fill.BackgroundColor = XLColor.LightGray;
                col++;
            }

            // Add data rows
            int row = 3;
            int serialNumber = 1;
            foreach (var item in updatedResult)
            {
                col = 1;
                worksheet.Cell(row, col).Value = serialNumber; // Serial number
                worksheet.Cell(row, col).Style.Font.Bold = true;
                worksheet.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                col++;
                foreach (var prop in typeof(CustomerGuestDownloadOut).GetProperties())
                {
                    var propValue = prop.GetValue(item);
                    var dataCell = worksheet.Cell(row, col);
                    dataCell.Value = propValue != null ? propValue.ToString() : "";

                    // Apply horizontal alignment to the cell
                    dataCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                    col++;
                }
                row++;
                serialNumber++;
            }

            //set defult Length of column
            for (int i = 1; i <= col; i++)
            {
                worksheet.Column(i).Width = 25; // Set the width as needed (in characters)
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);

                // Convert the MemoryStream to an IFormFile
                IFormFile excelFile = new FormFile(stream, 0, stream.Length, "data.xlsx", "data.xlsx");

                // Upload the IFormFile to Blob Storage
                string documentName = ((UploadDocumentType)12).ToString();

                var webFileOut = await _userFilesService.UploadWebFileOnGivenPathAsync(excelFile, documentName, cancellationToken, false);

                string test = webFileOut.TempSasUri.ToString();
                return _response.Success(new DownloadCustomerGuestOut("Get customer guests successful.", test));
            }
        }
    }
}
