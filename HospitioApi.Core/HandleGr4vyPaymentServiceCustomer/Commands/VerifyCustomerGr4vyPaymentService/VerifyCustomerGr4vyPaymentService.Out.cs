using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Commands.VerifyCustomerGr4vyPaymentService;

public class VerifyCustomerGr4vyPaymentServiceOut : BaseResponseOut
{
    public VerifyCustomerGr4vyPaymentServiceOut(string message, VerifyCustomerPaymentServiceOut verify) : base(message)
    {
        verifyCustomer = verify;
    }
    public VerifyCustomerPaymentServiceOut verifyCustomer { get; set; }
}
public class VerifyCustomerPaymentServiceOut
{
    public bool IsVerifySuccess { get; set; }
}