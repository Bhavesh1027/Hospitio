using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomers.Queries.GetGuestDefaultCheckinDetails
{
    public record GetGuestDefaultCheckinDetailsRequest(GetGuestDefaultCheckinDetailsIn In) : IRequest<AppHandlerResponse>;

    public class GetGuestDefaultCheckinDetailsHandler : IRequestHandler<GetGuestDefaultCheckinDetailsRequest, AppHandlerResponse>
    {
        private readonly IDapperRepository _dapper;
        private readonly IHandlerResponseFactory _response;

        public GetGuestDefaultCheckinDetailsHandler(IDapperRepository dapper, IHandlerResponseFactory response)
        {
            _dapper = dapper;
            _response = response;
        }

        public async Task<AppHandlerResponse> Handle(GetGuestDefaultCheckinDetailsRequest request, CancellationToken cancellationToken)
        {
            var spParams = new DynamicParameters();
            spParams.Add("CustomerId", request.In.CustomerId, DbType.Int32);

            var customer = await _dapper.GetSingle<GetGuestDefaultCheckinDetailsResponseOut>("[dbo].[GetCustomerCheckInPolicyById]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

            if (customer == null)
            {
                return _response.Error("Data not available", AppStatusCodeError.Gone410);
            }

            return _response.Success(new GetGuestDefaultCheckinDetailsOut("Get customer successful.", customer));
        }
    }
}
