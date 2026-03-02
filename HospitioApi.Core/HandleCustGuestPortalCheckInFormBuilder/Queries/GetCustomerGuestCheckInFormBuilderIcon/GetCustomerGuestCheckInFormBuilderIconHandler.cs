using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Queries.GetCustomerGuestPortalCheckInFormBuilder;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Queries.GetCustomerGuestCheckInFormBuilderIcon
{
    public record GetCustomerGuestPortalCheckInFormBuilderIconRequest(GetCustomerGuestCheckInFormBuilderIconIn In) : IRequest<AppHandlerResponse>;

    public class GetCustomerGuestCheckInFormBuilderIconHandler : IRequestHandler<GetCustomerGuestPortalCheckInFormBuilderIconRequest, AppHandlerResponse>
    {
        private readonly IDapperRepository _dapper;
        private readonly IHandlerResponseFactory _response;
        private readonly ApplicationDbContext _db;

        public GetCustomerGuestCheckInFormBuilderIconHandler(IDapperRepository dapper, IHandlerResponseFactory response, ApplicationDbContext db)
        {
            _dapper = dapper;
            _response = response;
            _db = db;
        }
        public async Task<AppHandlerResponse> Handle(GetCustomerGuestPortalCheckInFormBuilderIconRequest request, CancellationToken cancellationToken)
        {
            var spParams = new DynamicParameters();
            spParams.Add("CustomerId", request.In.CustomerId, System.Data.DbType.Int32);


            var result = await _dapper
                .GetSingle<CustomerGuestCheckInFormBuilderIconOut>("[dbo].[GetCustomerCheckInIcon]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);
            if (result == null)
            {
                return _response.Error("Data not available", AppStatusCodeError.Gone410);
            }
            return _response.Success(new GetCustomerGuestCheckInFormBuilderIconOut("Get customer Icon successful.", result));
        }
    }
}
