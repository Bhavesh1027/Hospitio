using ClosedXML.Excel;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using HospitioApi.Core.HandleCustomerGuest.Commands.DownloadCustomerGuest;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleMusement.Queries.DownloadMusementData;
public record DownloadMusementDataRequest() : IRequest<AppHandlerResponse>;
public class DownloadMusementDataHandler : IRequestHandler<DownloadMusementDataRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly IUserFilesService _userFilesService;

    public DownloadMusementDataHandler(
        IDapperRepository dapper,
        IHandlerResponseFactory response,
        IUserFilesService userFilesService
        )
    {
        _dapper = dapper;
        _response = response;
        _userFilesService = userFilesService;
    }
    public async Task<AppHandlerResponse> Handle(DownloadMusementDataRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("SearchString", "", DbType.String);
        spParams.Add("PageNo", 1, DbType.Int32);
        spParams.Add("PageSize", int.MaxValue, DbType.Int32);

        var musementData = await _dapper.GetAllJsonData<DownloadMusementData>("[dbo].[GetMusementData]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (musementData == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Data"); // Sheet name

            // Add headers
            int col = 1;
            worksheet.Cell(1, col).Value = "No";
            worksheet.Cell(1, col).Style.Font.Bold = true;
            worksheet.Cell(1, col).Style.Fill.BackgroundColor = XLColor.LightGray;
            col++;
            foreach (var prop in typeof(DownloadMusementData).GetProperties())
            {
                var headerCell = worksheet.Cell(1, col);
                headerCell.Value = prop.Name;
                if (prop.Name == "BusinessName")
                {
                    headerCell.Value = "Customer";
                }
                if (prop.Name == "Firstname")
                {
                    headerCell.Value = "Guest Name";
                }
                if (prop.Name == "Lastname")
                {
                    headerCell.Value = "Guest LastName";
                }
                if (prop.Name == "PaymentDate")
                {
                    headerCell.Value = "Date Purchased";
                }
                if (prop.Name == "MusementItemInfo")
                {
                    headerCell.Value = "Item Purchased";
                }
                if (prop.Name == "Amount")
                {
                    headerCell.Value = "Total Amount";
                }
                if (prop.Name == "MusementCancelItemInfo")
                {
                    headerCell.Value = "Cancelled Item";
                }
                // Apply CSS styling to the header cell
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Fill.BackgroundColor = XLColor.LightGray;
                col++;
            }

            // Add data rows
            int row = 3;
            int serialNumber = 1;
            foreach (var item in musementData)
            {
                col = 1;
                worksheet.Cell(row, col).Value = serialNumber; // Serial number
                worksheet.Cell(row, col).Style.Font.Bold = true;
                worksheet.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                col++;

                // Concatenate MusementItemInfo titles into a comma-separated string
                var musementItemInfoTitles = item.MusementItemInfo?.Select(info => info.Title).ToList();
                var musementItemInfoString = musementItemInfoTitles != null ? string.Join(", ", musementItemInfoTitles) : "";

                var musementCancelItemInfoTitles = item.MusementCancelItemInfo?.Select(info => info.Title).ToList();
                var musementCancelItemInfoString = musementCancelItemInfoTitles != null ? string.Join(", ", musementCancelItemInfoTitles) : "";

                foreach (var prop in typeof(DownloadMusementData).GetProperties())
                {
                    if (prop.Name != "MusementItemInfo" && prop.Name != "MusementCancelItemInfo")
                    {
                        var propValue = prop.GetValue(item);
                        var dataCell = worksheet.Cell(row, col);
                        dataCell.Value = propValue != null ? propValue.ToString() : "";
                        // Apply horizontal alignment to the cell
                        dataCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    }
                    else if(prop.Name == "MusementItemInfo")
                    {
                        worksheet.Cell(row, col).Value = musementItemInfoString;
                        worksheet.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    }
                    else
                    {
                        // Add the concatenated string to the Excel cell for MusementCancelItemInfo
                        worksheet.Cell(row, col).Value = musementCancelItemInfoString;
                        worksheet.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        col++;
                    }
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
                string documentName = ((UploadDocumentType)13).ToString();

                var webFileOut = await _userFilesService.UploadWebFileOnGivenPathAsync(excelFile, documentName, cancellationToken, false);

                string test = webFileOut.TempSasUri.ToString();
                return _response.Success(new DownloadCustomerGuestOut("Get customer guests successful.", test));
            }
        }
    }
}
