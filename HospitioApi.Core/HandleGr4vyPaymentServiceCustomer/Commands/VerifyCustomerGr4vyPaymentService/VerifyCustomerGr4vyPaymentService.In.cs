namespace HospitioApi.Core.HandleGr4vyPaymentServiceCustomer.Commands.VerifyCustomerGr4vyPaymentService;

public class VerifyCustomerGr4vyPaymentServiceIn
{
    public int? CustomerId { get; set; }
    public int PaymentProcessorId { get; set; }
    public List<Field> Fields { get; set; } = new List<Field>();
}
public class Field
{
    public string? key { get; set; }
    public string? value { get; set; }
}