using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomers
{
    public record GetCustomersRequest() : IRequest<AppHandlerResponse>;

    public class GetCustomersHandler : IRequestHandler<GetCustomersRequest, AppHandlerResponse>
    {
        private readonly IDapperRepository _dapper;
        private readonly IHandlerResponseFactory _response;
        public GetCustomersHandler(IDapperRepository dapper, IHandlerResponseFactory response)
        {
            _dapper = dapper;
            _response = response;
        }
        public async Task<AppHandlerResponse> Handle(GetCustomersRequest request, CancellationToken cancellationToken)
        {
            var spParams = new DynamicParameters();

            var customersInfo = await _dapper
                .GetAll<CustomerDetails>("[dbo].[GetCustomerDropDownDetails]", spParams, cancellationToken, CommandType.StoredProcedure);

            if (customersInfo == null || customersInfo.Count == 0)
            {
                return _response.Error("Data not available", AppStatusCodeError.Gone410);
            }
            return _response.Success(new GetCustomersOut("Get customer Details successful.", customersInfo));
        }

    }
}
