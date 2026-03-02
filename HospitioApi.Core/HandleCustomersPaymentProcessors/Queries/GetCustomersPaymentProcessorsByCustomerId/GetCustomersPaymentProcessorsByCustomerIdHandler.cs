using Dapper;
using MediatR;
using HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessors;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessorsUsingCustomerId;


public record GetCustomersPaymentProcessorsByCustomerIdRequest(GetCustomersPaymentProcessorsByCustomerIdIn In) : IRequest<AppHandlerResponse>;

    public class GetCustomersPaymentProcessorsByCustomerIdHandler : IRequestHandler<GetCustomersPaymentProcessorsByCustomerIdRequest, AppHandlerResponse>
    {

        private readonly IDapperRepository _dapper;
        private readonly IHandlerResponseFactory _response;

        public GetCustomersPaymentProcessorsByCustomerIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
        {
            _dapper = dapper;
            _response = response;
        }


        public async Task<AppHandlerResponse> Handle(GetCustomersPaymentProcessorsByCustomerIdRequest request, CancellationToken cancellationToken)
        {

          
            var spParams = new DynamicParameters();
           
            spParams.Add("Id", request.In.CustomerId, System.Data.DbType.Int32);

            var customersPaymentProcessors = await _dapper
                .GetAll<CustomersPaymentProcessorsByCustomerIdOut>("[dbo].[GetCustomePaymentProcessorsForPaymentProcessors]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

            if (customersPaymentProcessors == null || customersPaymentProcessors.Count == 0)
            {
                return _response.Error("Data not available", AppStatusCodeError.Gone410);
            }

            return _response.Success(new GetCustomersPaymentProcessorsByCustomerIdOut("Get customer payment processors successful.", customersPaymentProcessors));
        }
    }


   


