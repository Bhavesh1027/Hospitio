using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandlePaymentProcessors.Queries.GetPaymentProcessorByPaymentProcessorsId
{

    public record GetPaymentProcessorsDefinationsByPaymentProcessorsIdRequest(GetPaymentProcessorsDefinationsByPaymentProcessorsIdIn In) : IRequest<AppHandlerResponse>;
    public class GetPaymentProcessorsDefinationsByPaymentProcessorsIdHandler : IRequestHandler<GetPaymentProcessorsDefinationsByPaymentProcessorsIdRequest, AppHandlerResponse>
    {
        private readonly IDapperRepository _dapper;
        private readonly IHandlerResponseFactory _response;


        public GetPaymentProcessorsDefinationsByPaymentProcessorsIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
        {
            _dapper = dapper;
            _response = response;
        }

        public async Task<AppHandlerResponse> Handle(GetPaymentProcessorsDefinationsByPaymentProcessorsIdRequest request, CancellationToken cancellationToken)
        {
            var spParams = new DynamicParameters();
            spParams.Add("Id", request.In.PaymentProcessorId, DbType.Int32);

            var paymentProcessors = await _dapper.GetSingle<PaymentProcessorsDefinationsByPaymentProcessorsIdOut>("[dbo].[GetPaymentProcessorsDefinationsById]", spParams, cancellationToken, CommandType.StoredProcedure);


            if (paymentProcessors == null)
            {
                return _response.Error("Data not available", AppStatusCodeError.Gone410);
            }

            return _response.Success(new GetPaymentProcessorsDefinationsByPaymentProcessorsIdOut("Get payment processorDetailes successful.", paymentProcessors));
        }

    }
}

    
