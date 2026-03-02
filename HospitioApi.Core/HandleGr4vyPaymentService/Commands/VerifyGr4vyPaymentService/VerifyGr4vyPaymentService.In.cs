namespace HospitioApi.Core.HandleGr4vyPaymentService.Commands.VerifyGr4vyPaymentService;

public class VerifyGr4vyPaymentServiceIn
{
    public int PaymentProcessorId { get;set; }
    public List<Field> Fields { get; set; } = new List<Field>();
}
public class Field
{
    public string? key { get; set; }
    public string? value { get; set; }
}
