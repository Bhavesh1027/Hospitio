using HospitioApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Queries.GetCustomersPaymentProcessorsUsingCustomerId;


public class GetCustomersPaymentProcessorsByCustomerIdOut : BaseResponseOut
    {
        public GetCustomersPaymentProcessorsByCustomerIdOut(string message, List<CustomersPaymentProcessorsByCustomerIdOut>? _CustomersPaymentProcessorsByCustomerIdOut) : base(message)
        {
        CustomersPaymentProcessorsByCustomerIdOut = _CustomersPaymentProcessorsByCustomerIdOut;

        }
        public List<CustomersPaymentProcessorsByCustomerIdOut> CustomersPaymentProcessorsByCustomerIdOut { get; set; }

    }


    public class CustomersPaymentProcessorsByCustomerIdOut
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        public int? PaymentProcessorId { get; set; }

        public string? GRPaymentServiceId { get; set; }

        public bool? IsActive { get; set; }

        public string? GRCategory { get; set; }

        public string? GRGroup { get; set; }

        public string? GRID { get; set; }

        public string? GRIcon { get; set; }

        public string? GRName { get; set; }



    }

