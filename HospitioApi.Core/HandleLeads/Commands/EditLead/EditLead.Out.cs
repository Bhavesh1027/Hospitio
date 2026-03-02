using HospitioApi.Shared;


namespace HospitioApi.Core.HandleLeads.Commands.EditLead;
public class EditLeadOut : BaseResponseOut
{
    public EditLeadOut(string message, EditedLeadOut editedLeadOut) : base(message)
    {
        EditedLeadOut = editedLeadOut;
    }
    public EditedLeadOut EditedLeadOut { get; set; } = new();
}

public class EditedLeadOut
{

    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Company { get; set; }
    public string? Comment { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public int? ContactFor { get; set; }
    public bool? IsActive { get; set; }
}