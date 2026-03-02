using HospitioApi.Shared;


namespace HospitioApi.Core.HandleLeads.Commands.CreateLead;



public class CreateLeadOut : BaseResponseOut
{
    public CreateLeadOut(string message, CreatedLeadOut createdLeadOut) : base(message)
    {
        CreatedLeadOut = createdLeadOut;
    }
    public CreatedLeadOut CreatedLeadOut { get; set; } = new();
}

public class CreatedLeadOut
{

    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Company { get; set; }
    public string? Email { get; set; }
    public string? Comment { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public int? ContactFor { get; set; }
    public bool? IsActive { get; set; }

}
