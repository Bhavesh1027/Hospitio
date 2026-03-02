using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleTaxiTransfer.Queries.GetAllTransferData;
public record GetAllTransferDataRequest(GetAllTransferDataIn In) : IRequest<AppHandlerResponse>;
public class GetAllTransferDataHandler : IRequestHandler<GetAllTransferDataRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetAllTransferDataHandler(
        IDapperRepository dapper,
        IHandlerResponseFactory response
        )
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetAllTransferDataRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("SearchString", request.In.SearchValue, DbType.String);
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);
        spParams.Add("CustomerId", request.In.CustomerId, DbType.Int32);
        spParams.Add("GuestId", request.In.GuestId, DbType.Int32);
        spParams.Add("FromCreate", request.In.FromCreateAt, DbType.DateTime);
        spParams.Add("ToCreate", request.In.ToCreateAt, DbType.DateTime);

        var taxiTransferData = await _dapper.GetAllJsonData<TaxiTransferResponse>("[dbo].[GetAllTaxiTransferData]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (taxiTransferData == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetAllTransferDataOut("Get TaxiTransferData successful.", taxiTransferData));
    }
}
