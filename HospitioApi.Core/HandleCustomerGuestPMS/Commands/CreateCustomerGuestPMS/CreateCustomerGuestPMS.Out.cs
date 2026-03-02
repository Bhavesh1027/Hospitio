using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuestPMS.Commands.CreateCustomerGuestPMS;

public class CreateCustomerGuestPMSOut : BaseResponseOut
{
    public CreateCustomerGuestPMSOut(string message, CreatedCustomerGuestPMSOut createdCustomerGuestPMSOut) : base(message)
    {
        CreatedCustomerGuestPMSOut = createdCustomerGuestPMSOut;
    }
    public CreatedCustomerGuestPMSOut CreatedCustomerGuestPMSOut { get; set; }
}

public class CreatedCustomerGuestPMSOut
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DocumentName { get; set; }
    public string? Email { get; set; }
    public string FileName { get; set; }
}

