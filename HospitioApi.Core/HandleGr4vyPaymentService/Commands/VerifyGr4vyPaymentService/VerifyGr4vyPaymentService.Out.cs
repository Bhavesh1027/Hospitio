using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGr4vyPaymentService.Commands.VerifyGr4vyPaymentService;

public class VerifyGr4vyPaymentServiceOut : BaseResponseOut
{
    public VerifyGr4vyPaymentServiceOut(string message, VerifyPaymentServiceOut verifyPaymentServiceOut) : base(message)
    {
        verify = verifyPaymentServiceOut;
    }
    public VerifyPaymentServiceOut verify { get; set; }
}
public class VerifyPaymentServiceOut
{
    public bool IsVerifySuccess { get; set; }
}