using ClosedXML.Excel;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleTaxiTransfer.Queries.GetDownloadTransferData;
public record GetDownloadTransferDataRequest(GetDownloadTransferDataIn In) : IRequest<AppHandlerResponse>;
public class GetDownloadTransferDataHandler : IRequestHandler<GetDownloadTransferDataRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly IUserFilesService _userFilesService;

    public GetDownloadTransferDataHandler(
        IDapperRepository dapper,
        IHandlerResponseFactory response,
        IUserFilesService userFilesService
        )
    {
        _dapper = dapper;
        _response = response;
        _userFilesService = userFilesService;
    }
    public async Task<AppHandlerResponse> Handle(GetDownloadTransferDataRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("PageNo", 1, DbType.Int32);
        spParams.Add("PageSize", int.MaxValue, DbType.Int32);
        spParams.Add("Month", int.Parse(request.In.Month), DbType.Int32);
        spParams.Add("Year", int.Parse(request.In.Year), DbType.Int32);

        var downloadTaxiTransferData = await _dapper.GetAllJsonData<DownloadTaxiTransferResponse>("[dbo].[GetAllTaxiTransferData]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (downloadTaxiTransferData == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Data"); // Sheet name

            // Add headers
            int col = 1;
            worksheet.Cell(1, col).Value = "NUM";
            worksheet.Cell(1, col).Style.Font.Bold = true;
            worksheet.Cell(1, col).Style.Fill.BackgroundColor = XLColor.LightGray;
            worksheet.Cell(1, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(1, col).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(1, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            col++;
            foreach (var prop in typeof(DownloadTaxiTransferResponse).GetProperties())
            {
                var headerCell = worksheet.Cell(1, col);
                headerCell.Value = prop.Name;
                if (prop.Name == "CustomerName")
                {
                    headerCell.Value = "Customer";
                }
                if (prop.Name == "PickUpDateTime")
                {
                    headerCell.Value = "Booking Date";
                }
                if (prop.Name == "FareAmount")
                {
                    headerCell.Value = "Welcome Amount";
                }
                if (prop.Name == "MarkUpAmount")
                {
                    headerCell.Value = "Commission";
                }
                if (prop.Name == "HospitioFareAmount")
                {
                    headerCell.Value = "Total";
                }
                // Apply CSS styling to the header cell
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                col++;
            }

            // Add data rows
            int row = 3;
            int serialNumber = 1;
            foreach (var item in downloadTaxiTransferData)
            {
                col = 1;
                worksheet.Cell(row, col).Value = serialNumber; // Serial number
                worksheet.Cell(row, col).Style.Font.Bold = true;
                worksheet.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                col++;
                foreach (var prop in typeof(DownloadTaxiTransferResponse).GetProperties())
                {
                    var propValue = prop.GetValue(item);
                    var dataCell = worksheet.Cell(row, col);
                    if (prop.Name == "PickUpDateTime" && propValue != null)
                    {
                        if (DateTime.TryParse(propValue.ToString(), out DateTime parsedDate))
                        {
                            dataCell.Value = parsedDate.ToString("dd MMM yyyy, hh:mm tt");
                        }
                    }
                    else
                    {
                        dataCell.Value = propValue != null ? propValue.ToString() : "";
                    }

                    // Apply horizontal alignment to the cell
                    dataCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

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
                IFormFile excelFile = new FormFile(stream, 0, stream.Length, "taxiTransferData.xlsx", "taxiTransferData.xlsx");

                // Upload the IFormFile to Blob Storage
                string documentName = ((UploadDocumentType)16).ToString();

                var webFileOut = await _userFilesService.UploadWebFileOnGivenPathAsync(excelFile, documentName, cancellationToken, false);

                string test = webFileOut.TempSasUri.ToString();
                return _response.Success(new GetDownloadTransferDataOut("Download TaxiTransferData successful.", test));
            }
        }
    }
}
