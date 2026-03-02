using ClosedXML.Excel;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using HospitioApi.Core.HandleCustomerGuest.Commands.DownloadCustomerGuest;
using HospitioApi.Core.HandleLeads.Queries.GetLeadById;
using HospitioApi.Core.HandleLeads.Queries.GetLeads;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;
using static HospitioApi.Core.HandleLeads.Queries.GetLeads.GetLeadsOut;

namespace HospitioApi.Core.HandleLeads.Queries.DownloadLead;
public record DownloadLeadRequest() : IRequest<AppHandlerResponse>;
public class DownloadLeadHandler : IRequestHandler<DownloadLeadRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;
    private readonly IUserFilesService _userFilesService;
    public DownloadLeadHandler(IDapperRepository dapper,
        ApplicationDbContext db,
        IHandlerResponseFactory response,
        IUserFilesService userFilesService)
    {
        _db = db;
        _dapper = dapper;
        _response = response;
        _userFilesService = userFilesService;
    }

    public async Task<AppHandlerResponse> Handle(DownloadLeadRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("SearchValue", "", DbType.String);
        spParams.Add("PageNo", 1, DbType.Int32);
        spParams.Add("PageSize", int.MaxValue, DbType.Int32);
        spParams.Add("SortColumn", "Name", DbType.String);
        spParams.Add("SortOrder", "ASC", DbType.String);
        spParams.Add("AlphabetsStartsWith","", DbType.String);

        var downloadLeadsData = await _dapper.GetAll<DownloadLeadByOut>("[dbo].[GetLeads]"
                                    , spParams, cancellationToken,
        commandType: CommandType.StoredProcedure);

        if (downloadLeadsData == null || downloadLeadsData.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Forbidden403);
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
            foreach (var prop in typeof(DownloadLeadByOut).GetProperties())
            {
                var headerCell = worksheet.Cell(1, col);
                headerCell.Value = prop.Name;
                if (prop.Name == "Name")
                {
                    headerCell.Value = "Customer Name";
                }
                if (prop.Name == "CreatedAt")
                {
                    headerCell.Value = "Date Contacted";
                }
                // Apply CSS styling to the header cell
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Fill.BackgroundColor = XLColor.LightGray;
                col++;
            }

            // Add data rows
            int row = 3;
            int serialNumber = 1;
            foreach (var item in downloadLeadsData)
            {
                col = 1;
                worksheet.Cell(row, col).Value = serialNumber; // Serial number
                worksheet.Cell(row, col).Style.Font.Bold = true;
                worksheet.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                col++;
                foreach (var prop in typeof(DownloadLeadByOut).GetProperties())
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
                string documentName = ((UploadDocumentType)14).ToString();

                var webFileOut = await _userFilesService.UploadWebFileOnGivenPathAsync(excelFile, documentName, cancellationToken, false);

                string test = webFileOut.TempSasUri.ToString();
                return _response.Success(new DownloadCustomerGuestOut("Get leads successful.", test));
            }
        }
    }
}
