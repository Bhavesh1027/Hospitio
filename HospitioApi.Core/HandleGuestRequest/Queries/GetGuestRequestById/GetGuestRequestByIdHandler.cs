using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleGuestRequest.Queries.GetGuestRequestById;
public record GetGuestRequestByIdRequest(GetGuestRequestByIdIn In) : IRequest<AppHandlerResponse>;

public class GetGuestRequestByIdHandler : IRequestHandler<GetGuestRequestByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetGuestRequestByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetGuestRequestByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var guestRequestByIdOut = await _dapper
            .GetSingle<GuestRequestByIdOut>("[dbo].[GetGuestRequests]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (guestRequestByIdOut == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetGuestRequestByIdOut("Get guest request by id successful.", guestRequestByIdOut));
    }
}
