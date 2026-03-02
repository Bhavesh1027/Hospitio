using HospitioApi.Core.HandlePaymentProcessors.Queries.GetPaymentProcessorsById;
using HospitioApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandlePaymentProcessors.Queries.GetPaymentProcessorByPaymentProcessorsId
{


    public class GetPaymentProcessorsDefinationsByPaymentProcessorsIdOut : BaseResponseOut
    {

        public GetPaymentProcessorsDefinationsByPaymentProcessorsIdOut(string message, PaymentProcessorsDefinationsByPaymentProcessorsIdOut PaymentProcessorsDefinationsByPaymentProcessorsIdOut) : base(message)
        {
            _PaymentProcessorsDefinationsByPaymentProcessorsIdOut = PaymentProcessorsDefinationsByPaymentProcessorsIdOut;
        }
        public PaymentProcessorsDefinationsByPaymentProcessorsIdOut _PaymentProcessorsDefinationsByPaymentProcessorsIdOut { get; set; }


    }


    public class PaymentProcessorsDefinationsByPaymentProcessorsIdOut
    {
        public string? GRFields { get; set; }

        public string? GRSupportedCountries { get; set; }


        public string? GRSupportedCurrencies { get; set; }

        public string? GRSupportedFeatures { get; set; }

        public int PaymentProcessorId { get; set; }

    }
}
