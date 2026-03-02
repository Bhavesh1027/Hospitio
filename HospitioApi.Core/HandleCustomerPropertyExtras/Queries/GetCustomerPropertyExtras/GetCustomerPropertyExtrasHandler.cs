using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerPropertyExtras.Queries.GetCustomerPropertyExtras;
public record GetCustomerPropertyExtrasRequest(GetCustomerPropertyExtrasIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerPropertyExtrasHandler : IRequestHandler<GetCustomerPropertyExtrasRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerPropertyExtrasHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerPropertyExtrasRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerPropertyInformationId", request.In.CustomerPropertyInformationId, DbType.Int32);
        var result = await _dapper.GetAllJsonData<CustomerPropertyExtraOut>("[dbo].[GetCustomerPropertyExtras]"
     , spParams, cancellationToken,
     commandType: CommandType.StoredProcedure);

        if (result == null)
        {
            return _response.Error("Customers property extra could not be found", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerPropertyExtrasOut("Get customers property extra successful.", result));
    }
}
