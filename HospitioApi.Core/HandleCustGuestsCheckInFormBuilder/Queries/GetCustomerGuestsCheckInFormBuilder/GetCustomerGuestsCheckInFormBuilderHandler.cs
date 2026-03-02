using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustGuestsCheckInFormBuilder.Queries.GetCustomerGuestsCheckInFormBuilder;
public record GetCustomerGuestsCheckInFormBuilderRequest(GetCustomerGuestsCheckInFormBuilderIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerGuestsCheckInFormBuilderHandler : IRequestHandler<GetCustomerGuestsCheckInFormBuilderRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerGuestsCheckInFormBuilderHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerGuestsCheckInFormBuilderRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerId", request.In.CustomerId, System.Data.DbType.Int32);


        var result = await _dapper
            .GetAllJsonData<GetCustomerGuestsCheckInFormBuilderResponseOut>("[dbo].[GetCustomerGuestsCheckInFormBuilder]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (result == null || result.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomerGuestsCheckInFormBuilderOut("Get customer guests successful.", result[0]));
    }
}