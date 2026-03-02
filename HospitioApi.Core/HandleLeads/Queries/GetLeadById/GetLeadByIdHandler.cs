using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleLeads.Queries.GetLeadById;
public record GetLeadByIdRequest(GetLeadByIdIn In) : IRequest<AppHandlerResponse>;
public class GetLeadByIdHandler : IRequestHandler<GetLeadByIdRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;

    public GetLeadByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _response = response;
        _dapper = dapper;
    }

    public async Task<AppHandlerResponse> Handle(GetLeadByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var leadsDapperOuts = await _dapper.GetSingle<LeadByIdOut>("[dbo].[GetLeadById]"
                                    , spParams, cancellationToken,
        commandType: CommandType.StoredProcedure);

        if (leadsDapperOuts == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Forbidden403);
        }

        return _response.Success(new GetLeadByIdOut("Get lead successful.", leadsDapperOuts));
    }
}
